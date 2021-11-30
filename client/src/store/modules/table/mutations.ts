import { MutationTree } from "vuex";
import { TableState } from "@/models/store";
import {
  TableRow,
  TableHeader,
  TableCell,
  TableDataType,
  TableRowsSortType
} from "@/models/table";

export const mutations: MutationTree<TableState> = {
  setTable(
    state: TableState,
    payload: { rows: TableRow[]; headers: TableHeader[] }
  ): void {
    state.headers = payload.headers;
    state.rows = payload.rows;
    state.rows.push(
      new TableRow(
        payload.headers.map((item) => new TableCell(null, item.type))
      )
    );
  },
  pushRow(state: TableState, payload: { row: TableRow }): void {
    state.rows.push(payload.row);
  },
  deleteRow(state: TableState, payload: { index: number }): void {
    state.rows.splice(payload.index, 1);
  },
  swapRows(
    state: TableState,
    payload: { indexFirst: number; indexSecond: number }
  ): void {
    [state.rows[payload.indexFirst], state.rows[payload.indexSecond]] = [
      state.rows[payload.indexSecond],
      state.rows[payload.indexFirst]
    ];
  },
  pushHeader(state: TableState, payload: { header: TableHeader }): void {
    state.headers.push(payload.header);
  },
  deleteHeader(state: TableState, payload: { index: number }): void {
    state.headers.splice(payload.index, 1);
  },
  updateHeader(
    state: TableState,
    payload: { index: number; name?: string; type?: TableDataType }
  ): void {
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
      data?: string;
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
  swapHeaders(
    state: TableState,
    payload: { indexFirst: number; indexSecond: number }
  ): void {
    [state.headers[payload.indexFirst], state.headers[payload.indexSecond]] = [
      state.headers[payload.indexSecond],
      state.headers[payload.indexFirst]
    ];
  },
  pushCell(
    state: TableState,
    payload: { rowIndex: number; cell: TableCell }
  ): void {
    state.rows[payload.rowIndex].cells.push(payload.cell);
  },
  deleteCell(
    state: TableState,
    payload: { rowIndex: number; cellIndex: number }
  ): void {
    state.rows[payload.rowIndex].cells.splice(payload.cellIndex, 1);
  },
  swapCells(
    state: TableState,
    payload: { rowIndex: number; indexFirst: number; indexSecond: number }
  ): void {
    [
      state.rows[payload.rowIndex].cells[payload.indexFirst],
      state.rows[payload.rowIndex].cells[payload.indexSecond]
    ] = [
      state.rows[payload.rowIndex].cells[payload.indexSecond],
      state.rows[payload.rowIndex].cells[payload.indexFirst]
    ];
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
  sortRows(
    state: TableState,
    payload: { index: number; sortType: TableRowsSortType }
  ): void {
    const direction: number =
      payload.sortType === TableRowsSortType.Ascending ? 1 : -1;
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
          if (
            typeof numberFirst === "number" &&
            typeof numberSecond === "number"
          ) {
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
  setSortStatus(
    state: TableState,
    payload: { index: number; type: TableRowsSortType }
  ): void {
    state.sortStatus = {
      index: payload.index,
      type: payload.type
    };
  },
  clearSortStatus(state: TableState) {
    state.sortStatus = undefined;
  }
};
