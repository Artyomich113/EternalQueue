using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IInteractive
{
	//mouse
	void OnHold(Vector3 pos);

	//mouseUp
	void OnUp();

	// mouseDown
	void OnDown(Vector3 pos);

	//click
	void OnClick();
}

