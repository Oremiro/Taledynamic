import { State, TableState } from "@/models/store";
import {
  TableCell,
  TableRow,
  TableHeader,
  TableDataType,
  TableRowsSortType,
  TableData
} from "@/models/table";
import { ActionTree } from "vuex";

export const actions: ActionTree<TableState, State> = {
  async addRow({ state, commit }): Promise<void> {
    const row: TableRow = new TableRow(
      state.headers.map((item) => new TableCell(null, item.type))
    );
    commit("pushRow", { row: row });
  },
  async deleteRow(
    { commit, state },
    payload: { index: number }
  ): Promise<void> {
    if (payload.index < 0 || payload.index > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    commit("deleteRow", payload);
  },
  async addColumn(
    { commit, state },
    payload: { name: string; type: TableDataType }
  ): Promise<void> {
    commit("pushHeader", {
      header: new TableHeader(payload.name, payload.type)
    });
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("pushCell", {
        rowIndex: rowIndex,
        cell: new TableCell(null, payload.type)
      });
    }
  },
  async deleteColumn(
    { commit, state },
    payload: { index: number }
  ): Promise<void> {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Column index is out of range");
    }
    if (state.headers.length <= 1) {
      throw new Error("Table must contain at least one column");
    }
    commit("deleteHeader", payload);
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("deleteCell", { rowIndex: rowIndex, cellIndex: payload.index });
    }
  },
  async moveColumn(
    { commit, state },
    payload: { oldIndex: number; newIndex: number }
  ): Promise<void> {
    if (
      payload.oldIndex < 0 ||
      payload.oldIndex > state.headers.length - 1 ||
      payload.newIndex < 0 ||
      payload.newIndex > state.headers.length - 1
    ) {
      throw new Error("Index is out of range");
    }
    if (payload.oldIndex === payload.newIndex) {
      throw new Error("Indexes are the same");
    }
    commit("moveHeader", payload);
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("moveCell", {
        rowIndex: rowIndex,
        oldIndex: payload.oldIndex,
        newIndex: payload.newIndex
      });
    }
  },
  async moveRow(
    { commit, state },
    payload: { oldIndex: number; newIndex: number }
  ): Promise<void> {
    if (
      payload.oldIndex < 0 ||
      payload.oldIndex > state.rows.length - 1 ||
      payload.newIndex < 0 ||
      payload.newIndex > state.rows.length - 1
    ) {
      throw new Error("Index is out of range");
    }
    if (payload.oldIndex === payload.newIndex) {
      throw new Error("Indexes are the same");
    }
    commit("moveRow", payload);
    commit("clearSortStatus");
  },
  async sortRows(
    { commit, state },
    payload: { index: number; sortType: TableRowsSortType }
  ): Promise<void> {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    commit("sortRows", payload);
    commit("setSortStatus", { index: payload.index, type: payload.sortType });
  },
  async updateHeader(
    { state, commit },
    payload: { index: number; name?: string; type?: TableDataType }
  ): Promise<void> {
    commit("updateHeader", payload);
    if (payload.type !== undefined) {
      for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
        commit("updateCell", {
          rowIndex: rowIndex,
          cellIndex: payload.index,
          type: payload.type,
          data: ""
        });
      }
    }
  },
  async updateCell(
    { commit },
    payload: {
      rowIndex: number;
      cellIndex: number;
      data: TableData;
    }
  ): Promise<void> {
    commit("updateCell", payload);
  },
  async setColumnType(
    { state, commit },
    payload: { index: number; type: TableDataType }
  ): Promise<void> {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    const currentType: TableDataType = state.headers[payload.index].type;
    if (currentType === payload.type) return;
    commit("setHeaderType", payload);
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("setCellType", {
        rowIndex: rowIndex,
        cellIndex: payload.index,
        type: payload.type
      });
    }
  }
};
