// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateCommand.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the DelegateCommand type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.UI.Support
{
	using System;
	using System.Windows.Input;

	internal class DelegateCommand : ICommand
	{
		private readonly Func<object, bool> _canExecute;
		private readonly Action<object> _execute;

		public DelegateCommand(Func<object, bool> canExecute, Action<object> execute)
		{
			_canExecute = canExecute;
			_execute = execute;
		}

		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return _canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			if (_canExecute(parameter))
			{
				_execute(parameter);
			}
		}

		public void UpdateCanExecute()
		{
			var handler = CanExecuteChanged;
			if (handler != null)
			{
				CanExecuteChanged(this, EventArgs.Empty);
			}
		}
	}
}