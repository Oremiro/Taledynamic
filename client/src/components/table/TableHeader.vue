<template>
  <div
    style="
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 0.5rem 0.8rem;
      gap: 0.5rem;
    "
  >
    <div style="display: flex; gap: 0.5rem; align-items: center">
      <n-input
        v-model:value="header.name"
        autosize
        size="small"
        placeholder=""
        :style="{
          'background-color': themeVars.tableHeaderColor,
          'max-width': '10rem'
        }"
        @update:value="nameUpdateHandler"
        @mouseenter="
          store.commit('table/setEditableHeaderIndex', { index: index })
        "
        @mouseleave="store.commit('table/clearEditableHeaderIndex')"
      />
      <n-popconfirm
        v-if="store.getters['table/headers'].length > 1"
        v-model:show="isConfirmShown"
      >
        <template #icon>
          <n-icon :color="themeVars.errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="emit('delete')">
            Да
          </n-button>
          <n-button ghost size="small" @click="isConfirmShown = false">
            Нет
          </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button
            style="padding: 0 0.3rem"
            type="error"
            size="small"
            secondary
          >
            <n-icon size="1.1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить колонку?</div>
      </n-popconfirm>
    </div>
    <n-button
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
          transform:
            tableSortStatus?.type === 0 ? 'scale(-1, -1)' : 'scale(-1, 1)'
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
</template>

<script setup lang="ts">
import { ref, computed } from "vue";
import { useThemeVars, NPopconfirm } from "naive-ui";
import {
  DeleteIcon,
  ErrorCircleIcon,
  LinesSortIcon,
  LinesUnsortIcon
} from "@/components/icons";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { useStore } from "@/store";
import { TableHeader, TableRowsSortType } from "@/models/table";
import { TableSortStatus } from "@/models/store";
import { debounce } from "@/helpers";

const props = defineProps<{
  index: number;
}>();

const emit = defineEmits<{
  (e: "delete"): void;
}>();

const store = useStore();
const header = computed<TableHeader>(
  () => store.getters["table/headers"][props.index]
);
const isConfirmShown = ref<boolean>(false);

const tableSortStatus = computed<TableSortStatus | undefined>(
  () => store.getters["table/sortStatus"]
);

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

const nameUpdateHandler = debounce(async (value: string): Promise<void> => {
  try {
    await store.dispatch("table/updateHeader", { index: props.index, name: value });
  } catch (error) {
    if (error instanceof Error) {
      console.log(error.message);
    }
  }
}, 5000);

const themeVars = useThemeVars();
</script>
