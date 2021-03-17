using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Aranslow.GameObjects;
using Aranslow.Tools;

namespace Aranslow
{
    public class ObjectManager
    {
        public static ASLocalClient LocalPlayer;
        public static List<ASBaseClient> Objects;

        public static bool IsPopulated => Objects != null && Objects.Count >= 1 && LocalPlayer != null;

        public static void PopulateObjects()
        {
            if (!IsPopulated)
            {
                Logger.Log("Populating ObjectManager");

                LocalPlayer = new ASLocalClient(new Vector2(300, 300));

                Objects = new List<ASBaseClient>();
                Objects.Add(LocalPlayer);

                Logger.Log($"Successfully populated ObjectManager: {Objects.Count}");
            }
        }

        public delegate void ObjectManagementEvent(GameTime timeManaged, ASBaseClient objectManaged);

        public static event ObjectManagementEvent OnCreateObject;
        public static event ObjectManagementEvent OnDeleteObject;

        public static bool CreateObject<T>(Vector2 wPosition) where T : ASBaseClient
        {
            lock (Objects)
            {
                try
                {
                    var objCreated = new ASBaseClient(wPosition);

                    Objects.Add(objCreated);
                    OnCreateObject?.Invoke(Engine.SecondaryGameTimeHandle, objCreated);
                }
                catch (Exception) { return false; }
            }

            return true;
        }

        public static bool DeleteObject(ASBaseClient objToDelete)
        {
            lock (Objects)
            {
                try
                {
                    if (objToDelete != null && Objects.Contains(objToDelete))
                    {
                        Objects.Remove(objToDelete);
                        OnDeleteObject?.Invoke(Engine.SecondaryGameTimeHandle, objToDelete);
                    }
                    else
                        return false;
                }
                catch (Exception) { return false; }
            }

            return true;
        }
    }
}
