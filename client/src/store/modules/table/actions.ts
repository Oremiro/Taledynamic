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
  async setJsonTable({ commit }, payload: { dataId: string; jsonTable: string }): Promise<void> {
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
    commit("setDataId", { dataId: payload.dataId });
    commit("setTable", { headers: tableHeaders, rows: tableRows });
  },
  async addRow({ state, commit }): Promise<void> {
    const row: TableRow = new TableRow(state.headers.map((item) => new TableCell(null, item.type)));
    commit("pushRow", { row: row });
  },
  async deleteRow({ commit, state }, payload: { index: number }): Promise<void> {
    if (payload.index < 0 || payload.index > state.rows.length - 1) {
      throw new Error("Row index is out of range");
    }
    commit("deleteRow", payload);
  },
  async addColumn({ commit, state }, payload: { name: string; type: TableDataType }): Promise<void> {
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
  async deleteColumn({ commit, state }, payload: { index: number }): Promise<void> {
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
  async moveColumn({ commit, state }, payload: { oldIndex: number; newIndex: number }): Promise<void> {
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
  },
  async sortRows({ commit, state }, payload: { index: number; sortType: TableRowsSortType }): Promise<void> {
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
  async setColumnType({ state, commit }, payload: { index: number; type: TableDataType }): Promise<void> {
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
  },
  async pullTable({ dispatch, rootGetters }, payload: { tableId: number }): Promise<void> {
    await dispatch("user/refreshExpired", null, { root: true });
    try {
      const { data } = await TableDataApi.get({ id: payload.tableId }, rootGetters["user/accessToken"]);
      await dispatch("setJsonTable", { dataId: data.item.uId, jsonTable: data.item.tableData });
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.log(error.response?.status);
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
    } catch (error) {
      if (axios.isAxiosError(error)) {
        console.log(error.response?.status);
      }
    }
  }
};
