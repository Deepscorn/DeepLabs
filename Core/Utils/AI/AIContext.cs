// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UniRx;

namespace Utils.AI
{
	public class AiContext<TAiContext> where TAiContext : AiContext<TAiContext>
	{
		// State must not be assignable from client code. It is controlled from
		// states themselves. Or maybe later it will be controlled from Context. But not client code
		private AiState state;

		public AiContext(AiState initialState)
		{
			state = initialState;
			state.EnterState(GetTemplateInstance());
		}

		public void Update()
		{
			state.Update(GetTemplateInstance());
		}

		private void ChangeState(AiState newState)
		{
			state.LeaveState(GetTemplateInstance());
			state = newState;
			state.EnterState(GetTemplateInstance());
		}

		private TAiContext GetTemplateInstance()
		{
			return (TAiContext) this;
		}

		public class AiState
		{
			private readonly BehaviorSubject<bool> stateEnteredSubject = new BehaviorSubject<bool>(false);
			// for future, real state. Can be used to group different AiState logics per state
			//public virtual AIStateType Type { get; protected set; }

			// Do something before we transition to this state
			public virtual void EnterState(TAiContext context)
			{
				stateEnteredSubject.OnNext(true);
			}

			// Do something before we leave this state
			public virtual void LeaveState(TAiContext context)
			{
				stateEnteredSubject.OnNext(false);
			}

			// Process the input - causes state actions, may cause state transition
			public virtual void Update(TAiContext context)
			{
			}

			protected IObservable<T> UntilLeave<T>(IObservable<T> observable)
			{
				return observable.TakeUntil(stateEnteredSubject.Where(entered => !entered));
			}

			protected virtual void ChangeState(TAiContext context, AiState newState)
			{
				context.ChangeState(newState);
			}
		}
	}
}