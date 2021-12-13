import { State, TableSortStatus, TableState } from "@/models/store";
import { TableHeader, TableJson, TableRow } from "@/models/table";
import { GetterTree } from "vuex";

export const getters: GetterTree<TableState, State> = {
  headers(state: TableState): TableHeader[] {
    return state.headers;
  },
  rows(state: TableState): TableRow[] {
    return state.rows;
  },
  editableRowIndex(state: TableState): number | undefined {
    return state.editableRowIndex;
  },
  editableHeaderIndex(state: TableState): number | undefined {
    return state.editableHeaderIndex;
  },
  sortStatus(state: TableState): TableSortStatus | undefined {
    return state.sortStatus;
  },
  tableJson(state: TableState): TableJson {
    return JSON.parse(JSON.stringify({ headers: state.headers, rows: state.rows.slice(0, -1) }));
  }
};
