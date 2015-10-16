using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.Collections;

public class Slot : MonoBehaviour, IDropHandler {
	public int id;

	public void OnDrop(PointerEventData eventData)
	{
		//OnDrop finishes before End Drag
		ItemData droppedItem = eventData.pointerDrag.GetComponent<ItemData>();
		if (InventoryManager.Instance.items[id].Id == -1) {
			InventoryManager.Instance.MoveItem(droppedItem, id);
		}
		else if (droppedItem.slot != id) {
			//TODO This is kinda stupid... kinda
			if(this.transform.childCount > 0) {
				Transform swapItem = this.transform.GetChild(0);
				ItemData swapItemData = swapItem.GetComponent<ItemData>();

				if (droppedItem.item.Combineable && droppedItem.item.CombineId1 == swapItemData.item.Id) {
					if (droppedItem.item.CombineResult == swapItemData.item.CombineResult) {
						Debug.Log ("Combineable item is " + droppedItem.item.Title + " and this slot item is " + swapItemData.item.Title);
						//TODO This needs serious refactoring
						if (InventoryManager.Instance.IsItemInInventory(ItemDatabase.Instance.FetchItemByID(swapItemData.item.CombineResult))) {
							Debug.Log ("It exists!");
						}
					}
				}

				swapItemData.slot = droppedItem.slot;
				InventoryManager.Instance.SwapItem(droppedItem, swapItem.GetComponent<ItemData>(), id);

				swapItem.transform.SetParent(InventoryManager.Instance.slots[droppedItem.slot].transform);
				swapItem.transform.position = InventoryManager.Instance.slots[droppedItem.slot].transform.position;

				droppedItem.slot = id;
				droppedItem.transform.SetParent(this.transform);
				droppedItem.transform.position = this.transform.position;
			}
		}
	}
}
