﻿using EFT;
using EFT.UI;
using EFT.UI.Matchmaker;
using Fika.Core.Coop.Utils;
using Fika.Core.UI.Custom;
using SPT.Reflection.Patching;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Fika.Core.UI.Patches.MatchmakerAcceptScreen
{
	public class MatchmakerAcceptScreen_Show_Patch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return typeof(MatchMakerAcceptScreen).GetMethods(BindingFlags.Public | BindingFlags.Instance)
				.First(x => x.Name == "Show" && x.GetParameters()[0].Name == "session");
		}

		public static GameObject MatchmakerObject { get; set; }

		[PatchPrefix]
		private static void Prefix(ref RaidSettings raidSettings, DefaultUIButton ____acceptButton, DefaultUIButton ____backButton)
		{
			if (MatchmakerObject == null)
			{
				MatchmakerObject = new GameObject("MatchmakerObject");
			}

			if (raidSettings.Side == ESideType.Savage)
			{
				raidSettings.RaidMode = ERaidMode.Local;
			}

			MatchMakerUIScript newMatchMaker = MatchmakerObject.GetOrAddComponent<MatchMakerUIScript>();
			newMatchMaker.raidSettings = raidSettings;
			newMatchMaker.acceptButton = ____acceptButton;
			newMatchMaker.backButton = ____backButton;
		}

		[PatchPostfix]
		private static void Postfix(ref ISession session, Profile ___profile_0, MatchMakerAcceptScreen __instance)
		{
			FikaBackendUtils.MatchMakerAcceptScreenInstance = __instance;
			FikaBackendUtils.Profile = session.Profile;
			FikaBackendUtils.PMCName = session.Profile.Nickname;
		}
	}


}
