using System;
using HarmonyLib;
using JetBrains.Annotations;

namespace ONITwitchLib;

public static class EventInterface
{
	private const string EventManagerTypeName = "EventLib.EventManager, ONITwitch";
	private static Type eventManagerType;

	private const string EventInfoTypeName = "EventLib.EventInfo, ONITwitch";
	private static Type eventInfoType;
	
	private const string DataManagerTypeName = "EventLib.DataManager, ONITwitch";
	private static Type dataManagerType;

	/// <summary>
	/// Only safe to access if the Twitch mod is active.
	/// </summary>
	[NotNull]
	public static Type EventManagerType => (eventManagerType ??= Type.GetType(EventManagerTypeName))!;

	/// <summary>
	/// Only safe to access if the Twitch mod is active.
	/// </summary>
	[NotNull]
	public static Type EventInfoType => (eventInfoType ??= Type.GetType(EventInfoTypeName))!;

	/// <summary>
	/// Only safe to access if the Twitch mod is active.
	/// </summary>
	[NotNull]
	public static Type DataManagerType => (dataManagerType ??= Type.GetType(DataManagerTypeName))!;

	private static Func<object> eventManagerInstanceDelegate;

	/// <summary>
	/// Gets the instance of the event manager from the twitch mod.
	/// Only safe to access if the Twitch mod is active.
	/// </summary>
	public static EventManager GetEventManagerInstance()
	{
		if (eventManagerInstanceDelegate == null)
		{
			var prop = AccessTools.Property(EventManagerType, "Instance");
			var propInfo = prop.GetGetMethod();

			var retType = propInfo.ReturnType;
			if (retType != EventManagerType)
			{
				throw new Exception(
					$"The Instance property on {EventManagerType.AssemblyQualifiedName} does not return an instance of {EventManagerType}"
				);
			}

			// no argument because it's static property
			eventManagerInstanceDelegate = DelegateUtil.CreateDelegate<Func<object>>(propInfo, null);
		}

		var instance = eventManagerInstanceDelegate();
		return new EventManager(instance);
	}
	
	private static Func<object> dataManagerInstanceDelegate;

	/// <summary>
	/// Gets the instance of the data manager from the twitch mod.
	/// Only safe to access if the Twitch mod is active.
	/// </summary>
	public static DataManager GetDataManagerInstance()
	{
		if (dataManagerInstanceDelegate == null)
		{
			var prop = AccessTools.Property(DataManagerType, "Instance");
			var propInfo = prop.GetGetMethod();

			var retType = propInfo.ReturnType;
			if (retType != DataManagerType)
			{
				throw new Exception(
					$"The Instance property on {DataManagerType.AssemblyQualifiedName} does not return an instance of {DataManagerType}"
				);
			}

			// no argument because it's static property
			dataManagerInstanceDelegate = DelegateUtil.CreateDelegate<Func<object>>(propInfo, null);
		}

		var instance = dataManagerInstanceDelegate();
		return new DataManager(instance);
	}
}
