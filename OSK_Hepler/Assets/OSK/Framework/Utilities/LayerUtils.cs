using UnityEngine;

namespace OSK.Utils
{
    public static class LayerUtils
    {

        public const string LAYER_PLAYER = "Player";
        public const string LAYER_VICTIM = "Victim";
        public const string LAYER_AI = "AIBot";

        public static int GetDoorsLayerMask()
        {
            return LayerMask.GetMask(LAYER_PLAYER, LAYER_VICTIM);
        }

        public static int GetRadarAllLayerMask()
        {
            return ~LayerMask.GetMask(LAYER_VICTIM);
        }

        public static int GetVictimLayerMask()
        {
            return LayerMask.GetMask(LAYER_VICTIM);
        }

        public static int GetVictimLayer(bool isVictim)
        {
            return (isVictim) ? LayerMask.NameToLayer(LAYER_VICTIM) : LayerMask.NameToLayer(LAYER_PLAYER);
        }
    }
}