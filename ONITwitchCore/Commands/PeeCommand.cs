using ONITwitchCore.Toasts;

namespace ONITwitchCore.Commands;

public class PeeCommand : CommandBase
{
	public override bool Condition(object data)
	{
		return Components.LiveMinionIdentities.Count > 0;
	}

	public override void Run(object data)
	{
		foreach (var identity in Components.LiveMinionIdentities.Items)
		{
			var smi = identity.GetSMI<BladderMonitor.Instance>();
			if ((smi != null) && identity.TryGetComponent<ChoreProvider>(out var provider))
			{
				smi.GoTo(smi.sm.urgentwant);
				provider.AddChore(new PeeChore(identity.gameObject.GetComponent<StateMachineController>()));
			}
		}

		ToastManager.InstantiateToast(STRINGS.TOASTS.PEE.TITLE, STRINGS.TOASTS.PEE.BODY);
	}
}
