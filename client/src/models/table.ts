export enum TableDataType {
  Text,
  Number,
  Date,
  Image,
  File
}

export interface TableDataFile {
  uId: string | null;
  name: string;
}

export function isTableDataFile(item: TableData): item is TableDataFile {
  if (item === null) return false;
  return (item as TableDataFile).uId !== undefined && (item as TableDataFile).name !== undefined;
}

export type TableData = string | number | Date | TableDataFile | null;
export type TableDataJson = string | number | TableDataFile | null;

export enum TableRowsSortType {
  Ascending,
  Descending
}

export interface TableCellJson {
  data: TableDataJson;
  type: TableDataType;
}

export class TableCell {
  readonly id: symbol;
  data: TableData;
  type: TableDataType;

  constructor(data: TableData, type: TableDataType) {
    this.id = Symbol("id");
    if (data instanceof Date) {
      this.data = new Date(data.getTime());
    } else {
      this.data = data;
    }
    this.type = type;
  }
}

export interface TableHeaderJson {
  name: string;
  type: TableDataType;
}

export class TableHeader {
  readonly id: symbol;
  name: string;
  type: TableDataType;

  constructor(name: string, type: TableDataType) {
    this.id = Symbol("id");
    this.name = name;
    this.type = type;
  }
}

export interface TableRowJson {
  cells: TableCellJson[];
}

export class TableRow {
  readonly id: symbol;
  cells: TableCell[];

  constructor(cells: TableCell[]) {
    this.id = Symbol("id");
    this.cells = cells.map((cell) => new TableCell(cell.data, cell.type));
  }
}

export interface TableJson {
  headers: TableHeaderJson[];
  rows: TableRowJson[];
  immutable?: boolean;
}
