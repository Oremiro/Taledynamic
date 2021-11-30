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
  async addRow({ state, commit }) {
    const row: TableRow = new TableRow(
      state.headers.map((item) => new TableCell(null, item.type))
    );
    commit("pushRow", { row: row });
  },
  async deleteRow({ commit, state }, payload: { index: number }) {
    if (payload.index < 0 || payload.index > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    commit("deleteRow", payload);
  },
  async addColumn(
    { commit, state },
    payload: { name: string; type: TableDataType }
  ) {
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
  async deleteColumn({ commit, state }, payload: { index: number }) {
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
  async swapColumns(
    { commit, state },
    payload: { indexFirst: number; indexSecond: number }
  ) {
    if (
      payload.indexFirst < 0 ||
      payload.indexFirst > state.headers.length - 1 ||
      payload.indexSecond < 0 ||
      payload.indexSecond > state.headers.length - 1
    ) {
      throw new Error("Index is out of range");
    }
    if (payload.indexFirst === payload.indexSecond) {
      throw new Error("Indexes are the same");
    }
    commit("swapHeaders", payload);
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("swapCells", {
        rowIndex: rowIndex,
        indexFirst: payload.indexFirst,
        indexSecond: payload.indexSecond
      });
    }
  },
  async swapRows(
    { commit, state },
    payload: { indexFirst: number; indexSecond: number }
  ) {
    if (
      payload.indexFirst < 0 ||
      payload.indexFirst > state.rows.length - 1 ||
      payload.indexSecond < 0 ||
      payload.indexSecond > state.rows.length - 1
    ) {
      throw new Error("Index is out of range");
    }
    if (payload.indexFirst === payload.indexSecond) {
      throw new Error("Indexes are the same");
    }
    commit("swapRows", payload);
    commit("clearSortStatus");
  },
  async sortRows(
    { commit, state },
    payload: { index: number; sortType: TableRowsSortType }
  ) {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    commit("sortRows", payload);
    commit("setSortStatus", { index: payload.index, type: payload.sortType });
  },
  async updateHeader(
    { state, commit },
    payload: { index: number; name?: string; type?: TableDataType }
  ) {
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
  ) {
    commit("updateCell", payload);
  }
};
