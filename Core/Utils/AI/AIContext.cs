// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0

using System;
using UniRx;

namespace Utils.AI
{
	public class AiContext<TAiContext> : IDisposable where TAiContext : AiContext<TAiContext>
	{
		// State must not be assignable from client code. It is controlled from
		// states themselves. Or maybe later it will be controlled from Context. But not client code
		protected AiState state;
		protected readonly BehaviorSubject<bool> stateEnterSubject = new BehaviorSubject<bool>(false);

		public AiContext(AiState initialState)
		{
			state = initialState;
			stateEnterSubject.OnNext(true);
			state.EnterState(GetTemplateInstance());
		}

		// call that function to calculate AI state
		public void Update()
		{
			state.Update(GetTemplateInstance());
		}

		public IObservable<T> UntilStateLeave<T>(IObservable<T> observable)
		{
			return observable.TakeUntil(stateEnterSubject.Where(enter => !enter));
		}

		// TODO bind to state update (?)

		private void ChangeState(AiState newState)
		{
			stateEnterSubject.OnNext(false);
			state.LeaveState(GetTemplateInstance());
			state = newState;
			stateEnterSubject.OnNext(true);
			state.EnterState(GetTemplateInstance());
		}

		private TAiContext GetTemplateInstance()
		{
			return (TAiContext) this;
		}

		public void Dispose()
		{
			stateEnterSubject.OnNext(false);
		}

		public class AiState
		{
			// for future, real state. Can be used to group different AiState logics per state
			//public virtual AIStateType Type { get; protected set; }

			// Do something before we transition to this state
			public virtual void EnterState(TAiContext context)
			{
			}

			// Do something before we leave this state
			public virtual void LeaveState(TAiContext context)
			{
			}

			// Process the input - causes state actions, may cause state transition
			public virtual void Update(TAiContext context)
			{
			}

			protected virtual void ChangeState(TAiContext context, AiState newState)
			{
				context.ChangeState(newState);
			}
		}
	}
}