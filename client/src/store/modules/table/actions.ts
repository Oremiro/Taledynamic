import { TableDataApi } from "@/helpers/api/tableData";
import { State, TableState } from "@/models/store";
import {
  TableCell,
  TableRow,
  TableHeader,
  TableDataType,
  TableRowsSortType,
  TableData,
  TableJson
} from "@/models/table";
import axios from "axios";
import { ActionTree } from "vuex";

export const actions: ActionTree<TableState, State> = {
  async setJsonTable({ state, commit }, payload: { dataId: string; jsonTable: string }): Promise<void> {
    const parsedTable: Partial<TableJson> = JSON.parse(payload.jsonTable);

    const tableRows: TableRow[] = [];
    if (parsedTable.rows !== undefined) {
      for (const parsedRow of parsedTable.rows) {
        const rowCells: TableCell[] = [];
        for (const parsedCell of parsedRow.cells) {
          rowCells.push(
            new TableCell(
              parsedCell.type === TableDataType.Date && typeof parsedCell.data === "string"
                ? new Date(parsedCell.data)
                : parsedCell.data,
              parsedCell.type
            )
          );
        }
        tableRows.push(new TableRow(rowCells));
      }
    }
    let tableHeaders: TableHeader[] = [];
    if (parsedTable.headers === undefined) {
      tableHeaders.push(new TableHeader("Column #1", TableDataType.Text));
    } else {
      tableHeaders = parsedTable.headers.map((parsedHeader) => new TableHeader(parsedHeader.name, parsedHeader.type));
    }

    state.isUpdated = true;
    commit("setDataId", { dataId: payload.dataId });
    commit("setTable", { headers: tableHeaders, rows: tableRows, immutable: parsedTable.immutable ?? false });
  },
  async addRow({ state, commit }): Promise<void> {
    const row: TableRow = new TableRow(state.headers.map((item) => new TableCell(null, item.type)));
    commit("pushRow", { row: row });
    state.isUpdated = false;
  },
  async deleteRow({ commit, state }, payload: { index: number }): Promise<void> {
    if (payload.index < 0 || payload.index > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    commit("deleteRow", payload);
    state.isUpdated = false;
  },
  async addColumn({ commit, state }, payload: { name: string; type: TableDataType }): Promise<void> {
    if (state.immutable) return;
    commit("pushHeader", {
      header: new TableHeader(payload.name, payload.type)
    });
    for (let rowIndex = 0; rowIndex < state.rows.length; rowIndex++) {
      commit("pushCell", {
        rowIndex: rowIndex,
        cell: new TableCell(null, payload.type)
      });
    }
    state.isUpdated = false;
  },
  async deleteColumn({ commit, state }, payload: { index: number }): Promise<void> {
    if (state.immutable) return;
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
    state.isUpdated = false;
  },
  async moveColumn({ commit, state }, payload: { oldIndex: number; newIndex: number }): Promise<void> {
    if (state.immutable) return;
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
    state.isUpdated = false;
  },
  async moveRow({ commit, state }, payload: { oldIndex: number; newIndex: number }): Promise<void> {
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
    state.isUpdated = false;
  },
  async sortRows({ commit, state }, payload: { index: number; sortType: TableRowsSortType }): Promise<void> {
    if (payload.index < 0 || payload.index > state.headers.length - 1) {
      throw new Error("Index is out of range");
    }
    commit("sortRows", payload);
    commit("setSortStatus", { index: payload.index, type: payload.sortType });
    state.isUpdated = false;
  },
  async updateHeader(
    { state, commit },
    payload: { index: number; name?: string; type?: TableDataType }
  ): Promise<void> {
    if (state.immutable) return;
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
    state.isUpdated = false;
  },
  async updateCell(
    { state, commit },
    payload: {
      rowIndex: number;
      cellIndex: number;
      data: TableData;
    }
  ): Promise<void> {
    commit("updateCell", payload);
    state.isUpdated = false;
  },
  async setColumnType({ state, commit }, payload: { index: number; type: TableDataType }): Promise<void> {
    if (state.immutable) return;
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
    commit("clearSortStatus");
    state.isUpdated = false;
  },
  async pullTable({ dispatch, rootGetters }, payload: { tableId: number }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    try {
      const { data } = await TableDataApi.get({ id: payload.tableId }, rootGetters["user/accessToken"]);
      await dispatch("setJsonTable", { dataId: data.item.uId, jsonTable: data.item.tableData });
    } catch (error) {
      if (axios.isAxiosError(error)) {
        throw new Error(error.message);
      }
    }
  },
  async pushTable({ dispatch, rootGetters, getters, state }): Promise<void> {
    if (state.dataId === undefined) return;
    await dispatch("user/refreshExpired", null, { root: true });
    try {
      await TableDataApi.update(
        { uId: state.dataId, jsonContent: getters["tableJson"] },
        rootGetters["user/accessToken"]
      );
      state.isUpdated = true;
    } catch (error) {
      if (axios.isAxiosError(error)) {
        throw new Error(error.message);
      }
    }
  }
};
