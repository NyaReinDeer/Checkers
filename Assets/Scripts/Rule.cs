using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum state
{
	empty,
	white,
	black,
}
[System.Serializable]
public class Cell
{
	public RectTransform pos;
	public RectTransform posCell;
	public string name;
	public state st = state.empty;
	public bool qween = false;
	public bool borde = false;
}
public class Rule : MonoBehaviour
{
	public int N, M;
	public Cell[] ClassArr = new Cell[64];
	public int[,] IntArr = new int[8, 8];
	public bool whiteTime = false;
	public bool peace = true;
	public int bufA;
	public int lastX, lastY, x, y;
	public int countWhite = 12, countBlack = 12, cW = 12, cB = 12;
	public bool howQweenInRussian = true;
	public Transform del;
	public Text text;
	public state myCell, opCell;
	public List<int> listDel;
	public Sprite empty,qweenRed, qweenBlack,blueBorde,yellowBorde,redBorde;
	public void onClickEnter(int a)
	{
		x = a / 8;
		y = a % 8;
		bufA = a;
		if (ClassArr [a].borde) {
			if(peace){
				Swap(x, y, lastX, lastY);
				OffBorde();
				if(!ClassArr [a].qween && ((whiteTime && y==7)||(!whiteTime && y==0)))
					OnQween(a);
				Invers();
			}
			else{
				Swap(x, y, lastX, lastY);
				DeleteSo(x,y,lastX,lastY);
				OffBorde();
				if(!ClassArr [a].qween && ((whiteTime && y==7)||(!whiteTime && y==0)))
					OnQween(a);
				if(ClassArr[a].qween){
					if(!OnBordeVariableQween(x,y)){
						DeleteOver();
						Invers();
					}
				}
				if(!ClassArr[a].qween){
					if(!OnBordeVariable(x,y)){
						DeleteOver();
						Invers();
					}
				}
			}

		} else if(ClassArr [a].st==myCell){
			peace = ifPeace ();
			OffBorde();
			if(peace){
				if(ClassArr[a].qween){
					PeaceOnQweenVariable(x,y);
				}
				else{
					PeaceOnVariable(x,y);
				}
			}
			else{
				if(ClassArr[a].qween){
					OnBordeVariableQween(x,y);
				}
				else{
					OnBordeVariable(x,y);
				}
			}
		}
		lastX=x;
		lastY=y;
	}
	bool ifPeace()
	{
		int q = 0, bufCount;
		if (whiteTime)
		{
			myCell = state.white;
			opCell = state.black;
			bufCount=countWhite;
		}
		else
		{
			myCell = state.black;
			opCell = state.white;
			bufCount=countBlack;
		}
		for (int k = 0; k < bufCount; k++)
		{
			while (ClassArr[q].st != myCell) q++;
			int i = q / 8, j = q % 8;
			if (ClassArr[IntArr[i, j]].qween)
			{
				int ti = i, tj = j;
				if (ti < 6 && tj < 6) {
					if(ClassArr [IntArr [ti+1, tj+1]].st==state.empty){
						/*do{
							ti++; tj++;
						}while (ti < 5 && tj < 5 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty);*/
						while(ti<6 && tj<6 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty){
							ti++;tj++;
						}
					}
					if (ClassArr [IntArr [ti+1, tj+1]].st == opCell && ClassArr [IntArr [ti + 2, tj + 2]].st == state.empty) {
						return false;
					}
				}
				ti = i; tj = j;
				if (ti > 1 && tj < 6) {
					if(ClassArr [IntArr [ti-1, tj+1]].st==state.empty){
						/*do{
							ti--; tj++;
						}while (ti > 2 && tj < 5 && ClassArr[IntArr[ti-1, tj+1]].st == state.empty);*/
						while(ti>1 && tj<6 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty){
							ti++;tj++;
						}
					}
					if (ClassArr [IntArr [ti-1, tj+1]].st == opCell && ClassArr [IntArr [ti - 2, tj + 2]].st == state.empty) {//3,7
						return false;
					}
				}
				ti = i; tj = j;
				if (ti < 6 && tj > 1) {
					if(ClassArr [IntArr [ti+1, tj-1]].st==state.empty){
						/*do{
							ti++; tj--;
						}while (ti < 5 && tj > 2 && ClassArr[IntArr[ti+1, tj-1]].st == state.empty);*/
						while(ti<6 && tj>1 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty){
							ti++;tj++;
						}
					}
					if (ClassArr [IntArr [ti+1, tj-1]].st == opCell && ClassArr [IntArr [ti + 2, tj - 2]].st == state.empty) {
						return false;
					}
				}
				ti = i; tj = j;
				if (ti > 1 && tj > 1) {
					if(ClassArr [IntArr [ti-1, tj-1]].st==state.empty){
						/*do{
							ti--; tj--;
						}while (ti > 2 && tj > 2 && ClassArr[IntArr[ti-1, tj-1]].st == state.empty);*/
						while(ti>1 && tj>1 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty){
							ti++;tj++;
						}
					}
					if (ClassArr [IntArr [ti-1, tj-1]].st == opCell && ClassArr [IntArr [ti - 2, tj - 2]].st == state.empty) {
						return false;
					}
				}
			}
			else
			{
				
				if (j < 6 && i < 6 && ClassArr[IntArr[i, j]].st == myCell && ClassArr[IntArr[i + 1, j + 1]].st == opCell && ClassArr[IntArr[i + 2, j + 2]].st == state.empty)
				{
					return false;
				}
				if (j < 6 && i > 1 && ClassArr[IntArr[i, j]].st == myCell && ClassArr[IntArr[i - 1, j + 1]].st == opCell && ClassArr[IntArr[i - 2, j + 2]].st == state.empty)
				{
					return false;
				}
				if (j > 1 && i < 6 && ClassArr[IntArr[i, j]].st == myCell && ClassArr[IntArr[i + 1, j - 1]].st == opCell && ClassArr[IntArr[i + 2, j - 2]].st == state.empty)
				{
					return false;
				}
				if (j > 1 && i > 1 && ClassArr[IntArr[i, j]].st == myCell && ClassArr[IntArr[i - 1, j - 1]].st == opCell && ClassArr[IntArr[i - 2, j - 2]].st == state.empty)
				{
					return false;
				}
			}
			q++;
		}
		return true;
	}
	void DeleteSo(int x,int y, int lastX, int lastY){
		print ("Добавляем в список");
		if (x > lastX && y > lastY) {
			listDel.Add ((x - 1) * 8 + y - 1);
			print ("Delete");
			print ((x - 1) * 8 + y - 1);
		}
		if (x > lastX && y < lastY) {
			listDel.Add ((x - 1) * 8 + y + 1);
			print ("Delete");
			print((x - 1) * 8 + y + 1);
		}
		if (x < lastX && y > lastY) {
			listDel.Add ((x + 1) * 8 + y - 1);
			print ("Delete");
			print ((x + 1) * 8 + y - 1);
		}
		if (x < lastX && y < lastY) {
			listDel.Add ((x + 1) * 8 + y + 1);
			print ("Delete");
			print ((x + 1) * 8 + y + 1);
		}
	}
	void PeaceOnQweenVariable(int x,int y){
			int ti = x, tj = y;
			for (ti = x + 1, tj = y + 1; ti < 8 && tj < 8 && ClassArr[IntArr[ti, tj]].st == state.empty; ti++, tj++)
			OnBorde(ti, tj, yellowBorde);
			for (ti = x - 1, tj = y + 1; ti >= 0 && tj < 8 && ClassArr[IntArr[ti, tj]].st == state.empty; ti--, tj++)
			OnBorde(ti, tj, yellowBorde);
			for (ti = x + 1, tj = y - 1; ti < 8 && tj >= 0 && ClassArr[IntArr[ti, tj]].st == state.empty; ti++, tj--)
			OnBorde(ti, tj, yellowBorde);
			for (ti = x - 1, tj = y - 1; ti >= 0 && tj >= 0 && ClassArr[IntArr[ti, tj]].st == state.empty; ti--, tj--)
			OnBorde(ti, tj, yellowBorde);
	}
	void PeaceOnVariable(int x, int y){
		if (y < 7 && whiteTime)
		{
			if (x == 0 && ClassArr[IntArr[x + 1, y + 1]].st == state.empty)
			{
				OnBorde(x + 1, y + 1, yellowBorde);
			}
			if (x > 0 && x < 7)
			{
				if (ClassArr[IntArr[x + 1, y + 1]].st == state.empty)
				{
					OnBorde(x + 1, y + 1, yellowBorde);
				}
				if (ClassArr[IntArr[x - 1, y + 1]].st == state.empty)
				{
					OnBorde(x - 1, y + 1, yellowBorde);
				}
			}
			if (x == 7 && ClassArr[IntArr[x - 1, y + 1]].st == state.empty)
			{
				OnBorde(x - 1, y + 1, yellowBorde);
			}
		}
		if (y > 0 && !whiteTime)
		{
			if (x == 0 && ClassArr[IntArr[x + 1, y - 1]].st == state.empty)
			{
				OnBorde(x + 1, y - 1, yellowBorde);
			}
			if (x > 0 && x < 7)
			{
				if (ClassArr[IntArr[x + 1, y - 1]].st == state.empty)
				{
					OnBorde(x + 1, y - 1, yellowBorde);
				}
				if (ClassArr[IntArr[x - 1, y - 1]].st == state.empty)
				{
					OnBorde(x - 1, y - 1, yellowBorde);
				}
			}
			if (x == 7 && ClassArr[IntArr[x - 1, y - 1]].st == state.empty)
			{
				OnBorde(x - 1, y - 1, yellowBorde);
			}
		}
	}
	void OnQween(int a){
		ClassArr [a].qween = true;
		if (whiteTime) {
			ClassArr [a].posCell.GetComponent<Image> ().sprite = qweenRed;
		} else {
			ClassArr[a].posCell.GetComponent<Image>().sprite=qweenBlack;
		}
	}
	void Swap(int x, int y, int lastX, int lastY)
	{
		state buf = ClassArr[IntArr[x, y]].st;
		ClassArr[IntArr[x, y]].st=ClassArr[IntArr[lastX, lastY]].st;
		ClassArr [IntArr [lastX, lastY]].st = buf;
		bool qween = ClassArr [IntArr [x, y]].qween;
		ClassArr[IntArr[x, y]].qween = ClassArr[IntArr[lastX, lastY]].qween;
		ClassArr[IntArr[lastX, lastY]].qween = qween;
		Sprite stateSprite = ClassArr [IntArr [x, y]].posCell.GetComponent<Image>().sprite;
		ClassArr[IntArr[x, y]].posCell.GetComponent<Image>().sprite = ClassArr[IntArr[lastX, lastY]].posCell.GetComponent<Image>().sprite;
		ClassArr[IntArr[lastX, lastY]].posCell.GetComponent<Image>().sprite = stateSprite;
	}
	bool OnBordeVariable(int x, int y)
	{
		bool flag = false;
		if (y < 6 && x < 6 && ClassArr[IntArr[x, y]].st == myCell && ClassArr[IntArr[x + 1, y + 1]].st == opCell && ClassArr[IntArr[x + 2, y + 2]].st == state.empty)
		{
			if(!listDel.Contains((x+1)*8+y+1)){
				OnBorde(x + 2, y + 2, redBorde);
				flag=true;
			}
		}
		if (y < 6 && x > 1 && ClassArr[IntArr[x, y]].st == myCell && ClassArr[IntArr[x - 1, y + 1]].st == opCell && ClassArr[IntArr[x - 2, y + 2]].st == state.empty)
		{
			if(!listDel.Contains((x-1)*8+y+1)){
				OnBorde(x - 2, y + 2, redBorde);
				flag=true;
			}
		}
		if (y > 1 && x < 6 && ClassArr[IntArr[x, y]].st == myCell && ClassArr[IntArr[x + 1, y - 1]].st == opCell && ClassArr[IntArr[x + 2, y - 2]].st == state.empty)
		{
			if(!listDel.Contains((x+1)*8+y-1)){
				OnBorde(x + 2, y - 2, redBorde);
				flag=true;
			}
		}
		if (y > 1 && x > 1 && ClassArr[IntArr[x, y]].st == myCell && ClassArr[IntArr[x - 1, y - 1]].st == opCell && ClassArr[IntArr[x - 2, y - 2]].st == state.empty)
		{
			if(!listDel.Contains((x-1)*8+y-1)){
				OnBorde(x - 2, y - 2, redBorde);
				flag=true;
			}
		}
		return flag;
	}
	void OnBorde(int x, int y, Sprite buf)
	{
		if (!ClassArr [bufA].qween && ((whiteTime && y == 7) || (!whiteTime && y == 0)))
			buf = blueBorde;
		ClassArr[IntArr[x, y]].pos.GetComponent<Image>().sprite = buf;
		ClassArr[IntArr[x, y]].borde = true;
	}
	void OffBorde()
	{
		for (int i = 0; i < N*M; i++)
		{
			if (ClassArr[i].borde)
			{
				ClassArr[i].pos.GetComponent<Image>().sprite = empty;
				ClassArr[i].borde = false;
			}
		}
	}
	void Invers(){
		whiteTime = !whiteTime;
		if (whiteTime)
		{
			myCell = state.white;
			opCell = state.black;
		}
		else
		{
			myCell = state.black;
			opCell = state.white;
		}
	}
	void DeleteOver(){
		for (int i=0; i<listDel.Count; i++) {
			int n=listDel[i]/8,m=listDel[i]%8;
			Delete(n,m);
		}
		listDel.Clear();
	}
	void Delete(int x, int y)
	{
		if (opCell == state.white) {
			countWhite--;
		}
		if (opCell == state.black) {
			countBlack--;
		}
		ClassArr[IntArr[x, y]].posCell.GetComponent<Image>().sprite = empty;
		ClassArr[IntArr[x, y]].st = state.empty;
	}
	bool OnBordeVariableQween(int x, int y)
	{
		bool flag=false;
		int ti = x, tj = y;
		if (ti < 6 && tj < 6) {
			if(ClassArr [IntArr [ti+1, tj+1]].st==state.empty && ti<5 &&tj<5){
				do{
					ti++; tj++;
				}while (ti < 5 && tj < 5 && ClassArr[IntArr[ti+1, tj+1]].st == state.empty);
			}
			if (ClassArr [IntArr [ti+1, tj+1]].st == opCell && ClassArr [IntArr [ti + 2, tj + 2]].st == state.empty) {
				if (!listDel.Contains ((ti+1) * 8 + tj+1)) {
					OnBorde (ti + 2, tj + 2, redBorde);
					flag=true;
				}
			}
		}
		ti = x; tj = y;
		if (ti > 1 && tj < 6) {
			if(ClassArr [IntArr [ti-1, tj+1]].st==state.empty && ti>2 && tj<5){
				do{
					ti--; tj++;
				}while (ti > 2 && tj < 5 && ClassArr[IntArr[ti-1, tj+1]].st == state.empty);
			}
			if (ClassArr [IntArr [ti-1, tj+1]].st == opCell && ClassArr [IntArr [ti - 2, tj + 2]].st == state.empty) {//(2;4)
				if (!listDel.Contains ((ti-1) * 8 + tj+1)) {
					OnBorde (ti - 2, tj + 2, redBorde);
					flag=true;
				}
			}
		}
		ti = x; tj = y;
		if (ti < 6 && tj > 1) {
			if(ClassArr [IntArr [ti+1, tj-1]].st==state.empty && ti<5 && tj>2){
				do{
					ti++; tj--;
				}while (ti < 5 && tj > 2 && ClassArr[IntArr[ti+1, tj-1]].st == state.empty);
			}
			if (ClassArr [IntArr [ti+1, tj-1]].st == opCell && ClassArr [IntArr [ti + 2, tj - 2]].st == state.empty) {
				if (!listDel.Contains ((ti+1) * 8 + tj-1)) {
					OnBorde (ti + 2, tj - 2, redBorde);
					flag=true;
				}
			}
		}
		ti = x; tj = y;
		if (ti > 1 && tj > 1) {
			if(ClassArr [IntArr [ti-1, tj-1]].st==state.empty && ti>2 && tj>2){
				do{
					ti--; tj--;
				}while (ti > 2 && tj > 2 && ClassArr[IntArr[ti-1, tj-1]].st == state.empty);
			}
			if (ClassArr [IntArr [ti-1, tj-1]].st == opCell && ClassArr [IntArr [ti - 2, tj - 2]].st == state.empty) {
				if (!listDel.Contains ((ti-1) * 8 + tj-1)) {
					OnBorde (ti - 2, tj - 2, redBorde);
					flag=true;
				}
			}
		}
		return flag;
	}
	void Awake()
	{
		int s = 0;
		for (int i = 0; i < N; i++)
		{
			for (int j = 0; j < M; j++)
			{
				IntArr[i, j] = s++;
			}
		}
	}
	void Update(){
		if (countBlack == 0) {
			text.text = "WhiteWin";
		}
		if (countWhite == 0) {
			text.text = "BlackWin";
			Quaternion buf=text.gameObject.transform.rotation;
			buf.x=180f;
			text.gameObject.transform.rotation=buf;

		}

	}
}
