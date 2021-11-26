import { MutationTree } from "vuex";
import { TableState } from "@/models/store";
import { TableRow, TableHeader, TableCell } from "@/models/table";

export const mutations: MutationTree<TableState> = {
  setTable(
    state: TableState,
    payload: { rows: TableRow[]; headers: TableHeader[] }
  ): void {
    state.rows = payload.rows;
    state.headers = payload.headers;
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
  }
};
