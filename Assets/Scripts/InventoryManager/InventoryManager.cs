using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent (typeof (InventoryData))]
public class InventoryManager : MonoSingleton<InventoryManager> {
	public GameObject inventoryPanel;
	public GameObject slotPanel;
	public GameObject inventorySlot;
	public GameObject inventoryItem;

	public int slotAmount;

	public List<Item> items = new List<Item>();
	public List<GameObject> slots = new List<GameObject>();
	
	void Start()
	{
		SetupComponents();
		SetupPanelSlots();
	}

	public void SetupComponents()
	{
		inventoryPanel = GameObject.Find("InventoryPanel");
		slotPanel = inventoryPanel.transform.FindChild("SlotPanel").gameObject;

		//TODO Project dependance, this just to make sure shits don't get mess up
		if (slotAmount <= 0 || slotAmount > 16) {
			slotAmount = 16;
		}
	}

	public void SetupPanelSlots()
	{
		if (items.Count != 0) {
			return;
		}

		//TODO This add items depend on slot amount, maybe it should just add available in inv items
		for (int i = 0; i < slotAmount; i++) {
			items.Add(new Item());

			GameObject slotObj = Instantiate(inventorySlot);
			slotObj.name = string.Format("Slot" + i);
			slots.Add(slotObj);
			slots[i].GetComponent<Slot>().id = i;
			slots[i].transform.SetParent(slotPanel.transform);	
		}

		AddItem(0, 2);
		AddItem(1, 3);
		AddItem(2, 1);
		AddItem(3, 2);

	}

	public void AddItem(int id, int amount)
	{
		Item itemToAdd = ItemDatabase.Instance.FetchItemByID(id);

		if (itemToAdd.Stackable && IsItemInInventory(itemToAdd)) {
			for (int i = 0; i < items.Count; i ++) {
				if (items[i].Id == id) {
					ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
					data.amount += amount;
					data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
					break;
				}
			}
		}
		else {
			for (int i = 0; i < items.Count; i++) {
				if(items[i].Id == -1) {
					items[i] = itemToAdd;
					slots[i].GetComponent<Image>().color = new Color32(64, 132, 242, 100);
					
					GameObject itemObj = Instantiate(inventoryItem);
					ItemData itemData = itemObj.GetComponent<ItemData>();
					itemData.item = itemToAdd;
					itemData.amount = amount;
					itemData.slot = i;

					itemObj.transform.SetParent(slots[i].transform);
					itemObj.transform.position = Vector2.zero;
					itemObj.transform.GetChild(0).GetComponent<Text>().text = itemData.amount.ToString();

					itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
					itemObj.name = itemToAdd.Title;
					break;
				}
			}
		}
	}

	public void RemoveItem(int id, int amount)
	{
		Item itemToRemove = ItemDatabase.Instance.FetchItemByID(id);
		if (IsItemInInventory(itemToRemove)) {
			for (int i = 0; i < items.Count; i ++) {
				if (items[i].Id == id) {
					ItemData data = slots[i].transform.GetChild(0).GetComponent<ItemData>();
					if (amount < data.amount) {
						data.amount -= amount;
						data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();
					}
					else if (amount >= data.amount) {
						slots[i].GetComponent<Image>().color = new Color32(103, 115, 131, 39);
						Destroy(slots[i].transform.GetChild(0).gameObject);
						items[i] = new Item();
					}
					else {
						//TODO I need a tester...
						Debug.Log("What the hell else can happen?");
					}
					break;
				}
			}
		}
		else {
			return;
		}
	}

	public void MoveItem(ItemData itemData, int slotId)
	{
		items[itemData.slot] = new Item();
		slots[itemData.slot].GetComponent<Image>().color = new Color32(103, 115, 131, 39);

		items[slotId] = itemData.item;
		slots[slotId].GetComponent<Image>().color = new Color32(64, 132, 242, 100);

		itemData.slot = slotId;
	}

	public void SwapItem(ItemData swapperItemData, ItemData swappeeItemData, int slotId) //LOL at the naming
	{
		items[swapperItemData.slot] = swappeeItemData.transform.GetComponent<ItemData>().item;
		items[slotId] = swapperItemData.item;
	}

	public void CombineItem(int combinerId, int combineeId, int amount)
	{

	}

	public void LoadItem(int id)
	{
		Item itemToAdd = ItemDatabase.Instance.FetchItemByID(id);

		for (int i = 0; i < items.Count; i++) {
			if(items[i].Id == -1) {
				items[i] = itemToAdd;
				GameObject itemObj = Instantiate(inventoryItem);

				itemObj.transform.SetParent(slots[i].transform);
				slots[i].GetComponent<Image>().color = new Color32(64, 132, 242,100);

				itemObj.GetComponent<Image>().sprite = itemToAdd.Icon;
				itemObj.transform.position = Vector2.zero;
				itemObj.name = itemToAdd.Title;

				ItemData data = itemObj.GetComponent<ItemData>();
				data.amount = itemToAdd.Value;
				data.transform.GetChild(0).GetComponent<Text>().text = data.amount.ToString();

				if (!itemToAdd.Stackable && data.amount > 1) {
					data.amount = 1;
				}
				break;
			}
		}
	}

	public bool IsItemInInventory(Item item)
	{
		for (int i = 0; i < items.Count; i++) {
			if (items[i].Id == item.Id) {
				return true;
			}
		}
		return false;
	}
}
