export class DraggableList {
  dragStartIndex?: number;
  dragEnterIndex?: number;

  constructor() {
    this.dragStartIndex = undefined;
    this.dragEnterIndex = undefined;
  }

  getDataIndex(dataTransfer: DataTransfer): number | undefined {
    const dataItemIndex: string = dataTransfer?.getData("itemIndex");
    const itemIndex: number = parseInt(dataItemIndex);
    if (!isNaN(itemIndex)) {
      return itemIndex;
    }
  }

  dragStartHandler(e: DragEvent, index: number): void {
    console.log('start')
    this.dragStartIndex = index;
    if (e.dataTransfer) {
      e.dataTransfer.setData("itemIndex", index.toString());
    }
  }

  dragOverHandler(e: MouseEvent): void {
    console.log(e.offsetX, e.offsetY);
  }

  dragEnterHandler(e: DragEvent, index: number): void {
    if (e.dataTransfer) {
      e.dataTransfer.effectAllowed = "move";
      e.dataTransfer.dropEffect = "move";
      const itemIndex = this.getDataIndex(e.dataTransfer);
      if (itemIndex === undefined) return;
      if (index !== itemIndex) {
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

  dropHandler(e: DragEvent, index: number, callback: (index: number, itemIndex: number) => void) {
    if (e.dataTransfer) {
      const itemIndex = this.getDataIndex(e.dataTransfer);
      if (itemIndex === undefined) return;
      if (index !== itemIndex) {
        callback(index, itemIndex);
      }
    }
  }
}
