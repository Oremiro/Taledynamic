import { State, TableState } from "@/models/store";
import {
  TableCell,
  TableRow,
  TableHeader,
  TableDataType
} from "@/models/table";
import { ActionTree } from "vuex";

export const actions: ActionTree<TableState, State> = {
  async addRow({ state, commit }) {
    const cells: TableCell[] = [];
    for (const header of state.headers) {
      cells.push(new TableCell("", header.type));
    }
    commit("pushRow", { row: new TableRow(cells) });
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
        cell: new TableCell("", payload.type)
      });
    }
  },
  async deleteColumn({ commit, state }, payload: { index: number }) {
    if (payload.index < 0 || payload.index > state.rows.length - 1) {
      throw new Error("Column index is out of range");
    }
    if (payload.index === 0) {
      throw new Error("Can't delete first column");
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
      payload.indexFirst > state.rows.length - 1 ||
      payload.indexSecond < 0 ||
      payload.indexSecond > state.rows.length - 1
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
  },
  // async updateHeader() {}
};
