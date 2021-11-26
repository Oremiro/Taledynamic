import { State, TableState } from "@/models/store";
import { TableHeader, TableRow } from "@/models/table";
import { GetterTree } from "vuex";

export const getters: GetterTree<TableState, State> = {
  headers(state: TableState): TableHeader[] {
    return state.headers;
  },
  rows(state: TableState): TableRow[] {
    return state.rows;
  },
};
