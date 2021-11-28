<template>
  <th
    scope="row"
    :class="{
      draggable: !last,
      start: draggableStatus === 'start',
      enter: draggableStatus === 'enter' && !last
    }"
    style="padding: 0 0.3rem"
  >
    <div style="display: flex; align-items: center; justify-content: center">
      <span v-if="!last">{{ index + 1 }}</span>
      <n-icon v-else size="1rem">
        <add-icon />
      </n-icon>
    </div>
  </th>
  <td
    v-for="(cell, cellIndex) in data"
    :key="cell.id"
    style="padding: 0"
    :draggable="store.getters['table/editableRowIndex'] !== index"
    @dragstart.prevent
  >
    <table-cell-vue
      :data="cell.data"
      :type="cell.type"
      :index="cellIndex"
      @update="cellUpdateHandler"
      @mouse-enter-cell="
        store.commit('table/setEditableRowIndex', { index: index })
      "
      @mouse-leave-cell="store.commit('table/clearEditableRowIndex')"
    />
  </td>
  <td style="padding: 0">
    <div
      v-if="!last"
      style="display: flex; align-items: center; justify-content: center"
    >
      <n-popconfirm v-if="!last" v-model:show="isConfirmDeletionShown">
        <template #icon>
          <n-icon :color="themeVars.errorColor">
            <error-circle-icon />
          </n-icon>
        </template>
        <template #action>
          <n-button ghost type="error" size="small" @click="deleteRow">
            Да
          </n-button>
          <n-button ghost size="small" @click="isConfirmDeletionShown = false">
            Нет
          </n-button>
        </template>
        <template #trigger>
          <dynamically-typed-button type="error" size="tiny" ghost @click.stop>
            <n-icon size="1rem">
              <delete-icon />
            </n-icon>
          </dynamically-typed-button>
        </template>
        <div>Удалить строку?</div>
      </n-popconfirm>
    </div>
  </td>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { NPopconfirm, useThemeVars } from "naive-ui";
import { AddIcon, DeleteIcon, ErrorCircleIcon } from "@/components/icons";
import TableCellVue from "@/components/table/TableCell.vue";
import DynamicallyTypedButton from "@/components/DynamicallyTypedButton.vue";
import { TableCell } from "@/models/table";
import { useStore } from "@/store";
import { debounce } from "@/helpers";

const props = defineProps<{
  index: number;
  last: boolean;
  draggableStatus?: "enter" | "start";
  data: TableCell[];
}>();

const emit = defineEmits<{
  (e: "update"): void;
}>();

const store = useStore();

async function deleteRow() {
  await store.dispatch("table/deleteRow", { index: props.index });
}
const isConfirmDeletionShown = ref<boolean>(false);

const cellsUpdates = ref<Map<number, string | number | Date>>(new Map());

const updateStoredCells = debounce(async (): Promise<void> => {
  for (const [key, value] of cellsUpdates.value) {
    try {
      await store.dispatch("table/updateCell", {
        rowIndex: props.index,
        cellIndex: key,
        data: value
      });
    } catch (error) {
      if (error instanceof Error) {
        console.log(error.message);
      }
    }
  }
  cellsUpdates.value.clear();
}, 2000);

async function cellUpdateHandler(
  index: number,
  data: string | number | Date
): Promise<void> {
  emit("update");
  cellsUpdates.value?.set(index, data);
  await updateStoredCells();
}

const themeVars = useThemeVars();
</script>

<style scoped lang="scss">
@import "@/components/table/style.scss";
.draggable.enter {
  border-left: 1px solid v-bind("themeVars.primaryColor");
}
</style>
