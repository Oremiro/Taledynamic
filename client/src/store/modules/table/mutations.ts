import { MutationTree } from "vuex";
import { TableState } from "@/models/store";
import { TableRow, TableHeader, TableCell, TableDataType, TableRowsSortType, TableData } from "@/models/table";

export const mutations: MutationTree<TableState> = {
  setTable(state: TableState, payload: { rows: TableRow[]; headers: TableHeader[]; immutable?: boolean }): void {
    state.headers = payload.headers;
    state.rows = payload.rows;
    if (payload.immutable !== undefined) {
      state.immutable = payload.immutable;
    }
    if (!state.immutable) {
      state.rows.push(new TableRow(payload.headers.map((item) => new TableCell(null, item.type))));
    }
  },
  setDataId(state: TableState, payload: { dataId: string }): void {
    state.dataId = payload.dataId;
  },
  pushRow(state: TableState, payload: { row: TableRow }): void {
    state.rows.push(payload.row);
  },
  deleteRow(state: TableState, payload: { index: number }): void {
    state.rows.splice(payload.index, 1);
  },
  moveRow(state: TableState, payload: { oldIndex: number; newIndex: number }): void {
    const removedItem: TableRow = state.rows.splice(payload.oldIndex, 1)[0];
    state.rows.splice(payload.newIndex, 0, removedItem);
  },
  pushHeader(state: TableState, payload: { header: TableHeader }): void {
    state.headers.push(payload.header);
  },
  deleteHeader(state: TableState, payload: { index: number }): void {
    state.headers.splice(payload.index, 1);
  },
  updateHeader(state: TableState, payload: { index: number; name?: string; type?: TableDataType }): void {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    if (payload.name !== undefined) {
      state.headers[payload.index].name = payload.name;
    }
    if (payload.type !== undefined) {
      state.headers[payload.index].type = payload.type;
    }
  },
  updateCell(
    state: TableState,
    payload: {
      rowIndex: number;
      cellIndex: number;
      data?: TableData;
      type?: TableDataType;
    }
  ) {
    if (payload.rowIndex < 0 || payload.rowIndex > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    if (payload.cellIndex < 0 || payload.cellIndex > state.headers.length - 1) {
      throw new Error("Cell index is out of range");
    }
    if (payload.data !== undefined) {
      state.rows[payload.rowIndex].cells[payload.cellIndex].data = payload.data;
    }
    if (payload.type !== undefined) {
      state.rows[payload.rowIndex].cells[payload.cellIndex].type = payload.type;
    }
  },
  moveHeader(state: TableState, payload: { oldIndex: number; newIndex: number }): void {
    const removedItem: TableHeader = state.headers.splice(payload.oldIndex, 1)[0];
    state.headers.splice(payload.newIndex, 0, removedItem);
  },
  pushCell(state: TableState, payload: { rowIndex: number; cell: TableCell }): void {
    state.rows[payload.rowIndex].cells.push(payload.cell);
  },
  deleteCell(state: TableState, payload: { rowIndex: number; cellIndex: number }): void {
    state.rows[payload.rowIndex].cells.splice(payload.cellIndex, 1);
  },
  moveCell(state: TableState, payload: { rowIndex: number; oldIndex: number; newIndex: number }): void {
    const removedItem: TableCell = state.rows[payload.rowIndex].cells.splice(payload.oldIndex, 1)[0];
    state.rows[payload.rowIndex].cells.splice(payload.newIndex, 0, removedItem);
  },
  setEditableRowIndex(state: TableState, payload: { index: number }): void {
    state.editableRowIndex = payload.index;
  },
  clearEditableRowIndex(state: TableState): void {
    state.editableRowIndex = undefined;
  },
  setEditableHeaderIndex(state: TableState, payload: { index: number }): void {
    state.editableHeaderIndex = payload.index;
  },
  clearEditableHeaderIndex(state: TableState): void {
    state.editableHeaderIndex = undefined;
  },
  sortRows(state: TableState, payload: { index: number; sortType: TableRowsSortType }): void {
    const direction: number = payload.sortType === TableRowsSortType.Ascending ? 1 : -1;
    const headerType: TableDataType = state.headers[payload.index].type;
    const emptyRow: TableRow | undefined = state.rows.pop();
    switch (headerType) {
      case TableDataType.Text: {
        state.rows.sort((itemFirst, itemSecond) => {
          const textFirst = itemFirst.cells[payload.index].data ?? "";
          const textSecond = itemSecond.cells[payload.index].data ?? "";
          if (typeof textFirst === "string" && typeof textSecond === "string") {
            return textFirst.localeCompare(textSecond) * direction;
          }
          return 0;
        });
        break;
      }
      case TableDataType.Number: {
        state.rows.sort((itemFirst, itemSecond) => {
          const numberFirst = itemFirst.cells[payload.index].data;
          const numberSecond = itemSecond.cells[payload.index].data;
          if (numberFirst === null) return -direction;
          if (numberSecond === null) return direction;
          if (typeof numberFirst === "number" && typeof numberSecond === "number") {
            return (numberFirst - numberSecond) * direction;
          }
          return 0;
        });
        break;
      }
      case TableDataType.Date: {
        state.rows.sort((itemFirst, itemSecond) => {
          const dateFirst = itemFirst.cells[payload.index].data;
          const dateSecond = itemSecond.cells[payload.index].data;
          if (dateFirst === null) return -direction;
          if (dateSecond === null) return direction;
          if (dateFirst instanceof Date && dateSecond instanceof Date) {
            return (dateFirst.getTime() - dateSecond.getTime()) * direction;
          }
          return 0;
        });
        break;
      }
    }
    if (emptyRow !== undefined) {
      state.rows.push(emptyRow);
    }
  },
  setSortStatus(state: TableState, payload: { index: number; type: TableRowsSortType }): void {
    state.sortStatus = {
      index: payload.index,
      type: payload.type
    };
  },
  clearSortStatus(state: TableState) {
    state.sortStatus = undefined;
  },
  setHeaderType(state: TableState, payload: { index: number; type: TableDataType }): void {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    state.headers[payload.index].type = payload.type;
  },
  setCellType(state: TableState, payload: { rowIndex: number; cellIndex: number; type: TableDataType }): void {
    if (payload.rowIndex < 0 || payload.rowIndex > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    if (payload.cellIndex < 0 || payload.cellIndex > state.headers.length - 1) {
      throw new Error("Cell index is out of range");
    }
    const cell: TableCell = state.rows[payload.rowIndex].cells[payload.cellIndex];
    if (cell.type !== payload.type) {
      if (payload.type === TableDataType.Text) {
        if (cell.type === TableDataType.Number && typeof cell.data === "number") {
          cell.data = cell.data.toString();
        } else if (cell.type === TableDataType.Date && cell.data instanceof Date) {
          cell.data = cell.data.toLocaleDateString("ru");
        } else {
          cell.data = null;
        }
      } else if (payload.type === TableDataType.Number) {
        if (cell.type === TableDataType.Text && typeof cell.data === "string") {
          const convertedData = Number(cell.data);
          if (!isNaN(convertedData)) {
            cell.data = convertedData;
          } else {
            cell.data = null;
          }
        } else if (cell.type === TableDataType.Date && cell.data instanceof Date) {
          cell.data = cell.data.getTime();
        } else {
          cell.data = null;
        }
      } else if (payload.type === TableDataType.Date) {
        if (cell.type === TableDataType.Text && typeof cell.data === "string") {
          const matchedDate = cell.data.match(/(\d{2,4}).(\d{2}).(\d{2,4})/);
          if (matchedDate !== null) {
            const convertedData = Date.parse(`${matchedDate[3]}-${matchedDate[2]}-${matchedDate[1]}`);
            if (!isNaN(convertedData)) {
              cell.data = new Date(convertedData);
            } else {
              cell.data = null;
            }
          } else {
            cell.data = null;
          }
        } else {
          cell.data = null;
        }
      } else {
        cell.data = null;
      }
    }
    cell.type = payload.type;
  }
};
