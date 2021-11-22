export enum TableDataType {
  Text,
  Number,
  Date,
  Attachment
}

export class TableCell {
  readonly id: symbol;
  data: string | number | Date;
  type: TableDataType;

  constructor(data: string | number | Date, type: TableDataType) {
    this.id = Symbol("id");
    if (data instanceof Date) {
      this.data = new Date(data.getTime());
    } else {
      this.data = data;
    }
    this.type = type;
  }
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

export class TableRow {
  readonly id: symbol;
  cells: TableCell[];

  constructor(cells: TableCell[]) {
    this.id = Symbol("id");
    this.cells = cells.map((cell) => new TableCell(cell.data, cell.type));
  }
}
