<template>
  <div style="display: flex; align-items: center; justify-content: space-between; padding: 0.5rem 0.8rem; gap: 2rem">
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-input
        v-model:value="header.name"
        :maxlength="100"
        autosize
        size="small"
        placeholder=""
        :style="{
          'background-color': themeVars.tableHeaderColor,
          'max-width': '10rem'
        }"
        @update:value="nameUpdateHandler"
        @mouseenter="store.commit('table/setEditableHeaderIndex', { index: index })"
        @mouseleave="store.commit('table/clearEditableHeaderIndex')"
      />
      <n-popconfirm v-if="store.getters['table/headers'].length > 1" v-model:show="isConfirmShown">
        <template #icon>
          <n-icon :color="themeVars.errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="emit('delete')"> Да </n-button>
          <n-button ghost size="small" @click="isConfirmShown = false"> Нет </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button style="padding: 0 0.3rem" type="error" size="small" secondary>
            <n-icon size="1.1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить колонку?</div>
      </n-popconfirm>
    </div>
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-skeleton v-if="typeSelectLoading" size="small" width="1.7rem" :sharp="false" />
      <n-popconfirm v-else :show="isTypeConfirmationShown" trigger="manual">
        <template #icon>
          <n-icon>
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="warning" size="small" @click="setColumnType(tempColumnType)"> Да </n-button>
          <n-button ghost size="small" @click="resetColumnType"> Нет </n-button>
        </template>
        <template #trigger>
          <n-popselect
            v-model:value="tempColumnType"
            trigger="click"
            :options="columnTypeOptions"
            :render-label="optionsRenderLabel"
            :loading="typeSelectLoading"
            placement="right"
            @update:value="onColumnTypeUpdate"
          >
            <n-button size="small" secondary style="padding: 0 0.3rem">
              <n-icon size="1.1rem">
                <text-icon v-if="columnType === 0" />
                <number-symbol-icon v-else-if="columnType === 1" />
                <calendar-icon v-else-if="columnType === 2" />
                <image-icon v-else-if="columnType === 3" />
                <document-icon v-else />
              </n-icon>
            </n-button>
          </n-popselect>
        </template>
        <span
          >Изменение типа может привести к потери <br />
          данных непустых ячеек. Продолжить?</span
        >
      </n-popconfirm>
      <n-button
        :disabled="header.type === 3 || header.type === 4"
        size="small"
        secondary
        :type="tableSortStatus?.index === index ? 'success' : 'default'"
        style="padding: 0 0.3rem"
        @click="sortRows"
      >
        <n-icon
          v-if="tableSortStatus?.index === index"
          size="1.1rem"
          :style="{
            transform: tableSortStatus?.type === 0 ? 'scale(-1, -1)' : 'scale(-1, 1)'
          }"
        >
          <lines-sort-icon />
        </n-icon>
        <n-icon
          v-else
          size="1.1rem"
          :style="{
            transform: `scale(-1, -1)`
          }"
        >
          <lines-unsort-icon />
        </n-icon>
      </n-button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, h, nextTick } from "vue";
import { useThemeVars, NPopconfirm, SelectGroupOption, SelectOption, NIcon, NSkeleton } from "naive-ui";
import { DeleteIcon, ErrorCircleIcon, LinesSortIcon, LinesUnsortIcon } from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { useStore } from "@/store";
import { TableDataType, TableHeader, TableRow, TableRowsSortType } from "@/models/table";
import { TableSortStatus } from "@/models/store";
import { CalendarIcon, DocumentIcon, ImageIcon, NumberSymbolIcon, TextIcon } from "@/components/icons";

const props = defineProps<{
  index: number;
}>();

const emit = defineEmits<{
  (e: "delete"): void;
}>();

const store = useStore();
const header = computed<TableHeader>(() => store.getters["table/headers"][props.index]);
const isConfirmShown = ref<boolean>(false);

const tableSortStatus = computed<TableSortStatus | undefined>(() => store.getters["table/sortStatus"]);

async function sortRows(): Promise<void> {
  try {
    let sortType: TableRowsSortType;
    if (props.index === tableSortStatus.value?.index) {
      sortType =
        tableSortStatus.value.type === TableRowsSortType.Ascending
          ? TableRowsSortType.Descending
          : TableRowsSortType.Ascending;
    } else {
      sortType = TableRowsSortType.Ascending;
    }
    await store.dispatch("table/sortRows", {
      index: props.index,
      sortType: sortType
    });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error);
    }
  }
}

async function nameUpdateHandler(value: string): Promise<void> {
  try {
    await store.dispatch("table/updateHeader", {
      index: props.index,
      name: value
    });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error.message);
    }
  }
}

const optionsRenderLabel = (option: SelectOption | SelectGroupOption) => {
  if (option.type === "group") return option.label;
  return [
    h(
      NIcon,
      {
        size: "1.1rem",
        style: {
          verticalAlign: "middle",
          marginRight: ".5rem"
        }
      },
      {
        default: () =>
          h(
            option.value === TableDataType.Text
              ? TextIcon
              : option.value === TableDataType.Number
              ? NumberSymbolIcon
              : option.value === TableDataType.Date
              ? CalendarIcon
              : option.value === TableDataType.Image
              ? ImageIcon
              : DocumentIcon
          )
      }
    ),
    option.label
  ];
};

const columnTypeOptions: SelectGroupOption[] = [
  {
    type: "group",
    label: "Тип колонки",
    key: "columnType",
    children: [
      {
        label: "Текст",
        value: TableDataType.Text
      },
      {
        label: "Число",
        value: TableDataType.Number
      },
      {
        label: "Дата",
        value: TableDataType.Date
      },
      {
        label: "Изображение",
        value: TableDataType.Image
      },
      {
        label: "Файл",
        value: TableDataType.File
      }
    ]
  }
];

async function isColumnEmpty(): Promise<boolean> {
  const rows: TableRow[] = store.getters["table/rows"];
  for (let row of rows) {
    if (row.cells[props.index].data !== null) {
      return false;
    }
  }
  return true;
}

const typeSelectLoading = ref<boolean>(false);

async function setColumnType(type: TableDataType) {
  isTypeConfirmationShown.value = false;
  try {
    await store.dispatch("table/setColumnType", {
      index: props.index,
      type: type
    });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error.message);
    }
  }
  columnType.value = type;
}

async function resetColumnType() {
  tempColumnType.value = columnType.value;
  isTypeConfirmationShown.value = false;
}

async function onColumnTypeUpdate(value: TableDataType): Promise<void> {
  if (value === columnType.value) return;
  typeSelectLoading.value = true;
  const isEmpty: boolean = await isColumnEmpty();
  typeSelectLoading.value = false;
  await nextTick();
  if (isEmpty) {
    setColumnType(value);
  } else {
    isTypeConfirmationShown.value = true;
  }
}

const columnType = ref<TableDataType>(store.getters["table/headers"][props.index].type);
const tempColumnType = ref<TableDataType>(columnType.value);

const isTypeConfirmationShown = ref<boolean>(false);

const themeVars = useThemeVars();
</script>
