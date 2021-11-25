interface ListTransferObject {
  itemIndex: number;
  dropZoneName: string;
}

function isListTransferObject(item: ListTransferObject): item is ListTransferObject {
  return (
    (item as ListTransferObject).itemIndex !== undefined &&
    (item as ListTransferObject).dropZoneName !== undefined
  );
}

export class DraggableList {
  dragStartIndex?: number;
  dragEnterIndex?: number;
  dropZoneName: string;

  constructor(dropZoneName: string) {
    this.dragStartIndex = undefined;
    this.dragEnterIndex = undefined;
    this.dropZoneName = dropZoneName;
  }

  dragStartHandler(e: DragEvent, index: number): void {
    this.dragStartIndex = index;
    if (e.dataTransfer) {
      e.dataTransfer.effectAllowed = "move";
      const transferObject: ListTransferObject = {
        itemIndex: index,
        dropZoneName: this.dropZoneName
      };
      e.dataTransfer.setData("transferString", JSON.stringify(transferObject));
    }
  }

  dragEnterHandler(e: DragEvent, index: number): void {
    if (e.dataTransfer) {
      e.dataTransfer.dropEffect = "move";
      const transferObject: ListTransferObject = JSON.parse(
        e.dataTransfer.getData("transferString")
      );
      if (!isListTransferObject(transferObject)) {
        return;
      }
      if (this.dropZoneName !== transferObject.dropZoneName) {
        return;
      }
      if (index !== transferObject.itemIndex) {
        this.dragEnterIndex = index;
      } else {
        this.dragEnterIndex = undefined;
      }
    }
  }

  dragEndHandler(): void {
    this.dragStartIndex = undefined;
    this.dragEnterIndex = undefined;
  }

  dropHandler(
    e: DragEvent,
    index: number,
    callback: (index: number, itemIndex: number) => void
  ) {
    if (e.dataTransfer) {
      const transferObject: ListTransferObject = JSON.parse(
        e.dataTransfer.getData("transferString")
      );
      if (!isListTransferObject(transferObject)) {
        return;
      }
      if (this.dropZoneName !== transferObject.dropZoneName) {
        console.log(this.dragEnterIndex)
        if (this.dragEnterIndex) {
          callback(index, this.dragEnterIndex);
        } else {
          return;
        }
      }
      if (index !== transferObject.itemIndex) {
        callback(index, transferObject.itemIndex);
      }
    }
  }
}
