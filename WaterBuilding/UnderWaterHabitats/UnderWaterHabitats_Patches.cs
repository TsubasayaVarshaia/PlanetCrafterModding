using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SpaceCraft;
using SpaceCraft.Network.Utilities.ClientAuthority;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BepInEx.Logging;

#nullable enable
namespace UnderWaterHabitats {
    [BepInPlugin("tsubasaya.theplanetcraftermods.underwaterhabitats", PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Underwaterhabitats_Patches : BaseUnityPlugin {

        // private static ManualLogSource logger;

        public void Awake() {
            // logger = Logger;
            // logger.LogDebug("Hello, World - Tsu");
            // logger.LogInfo("Hello, World! Info version!");
            Harmony.CreateAndPatchAll(typeof (Underwaterhabitats_Patches));
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(PlayerDirectEnvironment), "SendColliderInfos")]
        private static bool PlayerDirectEnvironment_PreColliderInfos(PlayerDirectEnvironment __instance) {
            List<DataConfig.HomemadeTag> colliderTags = Traverse.Create(__instance).Field("_collidersEnteredTags").GetValue() as List<DataConfig.HomemadeTag>;
            bool flag1 = colliderTags.Contains(DataConfig.HomemadeTag.IsUnderWater);
            bool flag2 = colliderTags.Contains(DataConfig.HomemadeTag.IsInsideLivable);

            if(flag1 && flag2) {
                PlayerUpdateAnimator pua = Traverse.Create(__instance).Field("_playerUpdateAnimator").GetValue() as PlayerUpdateAnimator;
                pua.StopSwimming();
                Traverse.Create(__instance).Field("_playerUpdateAnimator").SetValue(pua);

                PlayerMovable _playerMovable = Traverse.Create(__instance).Field("_playerMovable").GetValue() as PlayerMovable;
                _playerMovable.IsSwimming(false);
                Traverse.Create(__instance).Field("_playerMovable").SetValue(_playerMovable);

                PlayerGaugesHandler _playerGaugesHandler = Traverse.Create(__instance).Field("_playerGaugesHandler").GetValue() as PlayerGaugesHandler;
                _playerGaugesHandler.SetPlayerCanBreathe(true);
                _playerGaugesHandler.SetPlayerIsUnderWater(false);
                Traverse.Create(__instance).Field("_playerGaugesHandler").SetValue(_playerGaugesHandler);

                PlayerAudio _playerAudio = Traverse.Create(__instance).Field("_playerAudio").GetValue() as PlayerAudio;
                _playerAudio.SetUnderWater(false);
                _playerAudio.SetTouchingWater(false);
                Traverse.Create(__instance).Field("_playerAudio").SetValue(_playerAudio);

                return false;
            }
            return true;

            // bool flag1 = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.IsUnderWater);
            // if ((Object) this._playerUpdateAnimator != (Object) null)
            // {
            //     if (flag1)
            //     this._playerUpdateAnimator.StartSwimming();
            //     else
            //     this._playerUpdateAnimator.StopSwimming();
            // }
            // if ((Object) this._playerMovable == (Object) null)
            //     return;
            // bool flag2 = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.IsInsideLivable);
            // bool flag3 = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.IsInsideBreathableArea);
            // bool _isTouchingWater = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.IsTouchingWater);
            // bool shouldNotRain = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.ShouldNotRainHere);
            // bool _isInNoFlyZone = this._collidersEnteredTags.Contains(DataConfig.HomemadeTag.NoFlyZone);
            // this._playerMovable.IsSwimming(flag1);
            // this._playerGaugesHandler.SetPlayerCanBreathe(((!flag2 ? 0 : (!flag1 ? 1 : 0)) | (flag3 ? 1 : 0)) != 0);
            // this._playerGaugesHandler.SetPlayerIsUnderWater(flag1 && !flag3);
            // if ((Object) this._meteoHandler == (Object) null)
            //     this._meteoHandler = Managers.GetManager<MeteoHandler>();
            // if ((Object) this._meteoHandler != (Object) null)
            // {
            //     this._meteoHandler.SetIsInLivable(flag2 | flag1 | flag3);
            //     this._meteoHandler.SetShouldNotRain(shouldNotRain);
            // }
            // this._playerAudio.SetUnderWater(flag1);
            // this._playerAudio.SetTouchingWater(_isTouchingWater);
            // this._playerMovable.SetIsInNoFlyZone(_isInNoFlyZone);
        }
    }
}
