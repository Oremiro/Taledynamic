<template>
  <transition name="fade" mode="out-in">
    <div v-if="isInitializationSuccess && tables.length">
      <div style="display: flex; align-items: center; justify-content: space-between; margin-bottom: 1.5rem">
        <n-text>Ваши таблицы</n-text>
        <n-tag>{{ tables.length }} / 100</n-tag>
      </div>
      <div style="display: flex; gap: 1rem; flex-wrap: wrap">
        <table-creating-item
          v-if="tables.length < 100"
          :workspace-id="props.workspaceId"
          :tables-count="tables.length"
          tertiary
          type="primary"
          style="max-width: 12rem"
          @create="pushTableToList"
        >
          <n-icon size="1.2rem">
            <add-icon />
          </n-icon>
        </table-creating-item>
        <tables-list-item
          v-for="table of tables"
          :id="table.id"
          :key="table.id"
          :workspace-id="workspaceId"
          :name="table.name"
          :editable="editable"
          @update="updateListItem"
          @delete="deleteListItem"
        />
      </div>
    </div>
    <n-empty
      v-else-if="isInitializationSuccess && !tables.length"
      size="large"
      description="В выбранном рабочем пространстве нет таблиц"
      style="height: 15rem; display: flex; justify-content: center; text-align: start"
    >
      <template #icon>
        <n-icon>
          <apps-list-icon />
        </n-icon>
      </template>
      <template #extra>
        <div style="max-width: 15rem; text-align: initial">
          <table-creating-item
            :workspace-id="props.workspaceId"
            :tables-count="tables.length"
            @create="pushTableToList"
          >
            Создать таблицу
          </table-creating-item>
        </div>
      </template>
    </n-empty>
    <tables-loading-item
      v-else
      style="height: 15rem"
      :error="isInitializationError"
      @repeat-initialization="initializeTablesList"
    />
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, watch, computed } from "vue";
import { useMessage, NEmpty, NTag } from "naive-ui";
import { TableDto } from "@/models/api/responses";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { debounce } from "@/helpers";
import { InitializationStatus, TablesSortType } from "@/models";
import TablesLoadingItem from "@/components/tables/TablesLoadingItem.vue";
import TableCreatingItem from "@/components/tables/TableCreatingItem.vue";
import TablesListItem from "@/components/tables/TablesListItem.vue";
import { AddIcon, AppsListIcon } from "@/components/icons";

const props = defineProps<{
  workspaceId: number;
  editable?: boolean;
  sortType?: TablesSortType;
}>();

const tables = ref<TableDto[]>([]);

const store = useStore();
const message = useMessage();

const tablesInitializationStatus = ref<InitializationStatus>(InitializationStatus.Pending);

async function initializeTablesList(): Promise<void> {
  if (isNaN(props.workspaceId)) return;
  tablesInitializationStatus.value = InitializationStatus.Pending;
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await TableApi.getList(
      {
        workspaceId: props.workspaceId
      },
      store.getters["user/accessToken"]
    );
    tables.value = data.tables;
    tablesInitializationStatus.value = InitializationStatus.Success;
  } catch (error) {
    tablesInitializationStatus.value = InitializationStatus.Error;
    if (axios.isAxiosError(error)) {
      message.error(error.message);
    }
  }
}

async function pushTableToList(table: TableDto) {
  tables.value.push(table);
  await sortList(props.sortType ?? TablesSortType.NameAscending);
}

async function updateListItem(oldId: number, table: TableDto) {
  const indexFound = tables.value.findIndex((item) => item.id === oldId);
  if (~indexFound) {
    tables.value[indexFound] = table;
    await sortList(props.sortType ?? TablesSortType.NameAscending);
  }
}

function deleteListItem(id: number) {
  const indexFound = tables.value.findIndex((item) => item.id === id);
  if (~indexFound) {
    tables.value.splice(indexFound, 1);
  }
}

async function sortList(sortType: TablesSortType) {
  if (sortType === TablesSortType.NameAscending) {
    tables.value.sort((itemFirst, itemSecond) => itemFirst.name.localeCompare(itemSecond.name));
  } else if (sortType === TablesSortType.NameDescending) {
    tables.value.sort((itemFirst, itemSecond) => itemSecond.name.localeCompare(itemFirst.name));
  }
}

const isInitializationError = computed<boolean>(() => tablesInitializationStatus.value === InitializationStatus.Error);
const isInitializationSuccess = computed<boolean>(
  () => tablesInitializationStatus.value === InitializationStatus.Success
);

const initializeTableListDebounced = debounce(initializeTablesList, 1000);

watch(
  () => props.workspaceId,
  async () => {
    tablesInitializationStatus.value = InitializationStatus.Pending;
    await initializeTableListDebounced();
    await sortList(props.sortType ?? TablesSortType.NameAscending);
  },
  { immediate: true }
);

defineExpose({
  sortList,
  tables
});
</script>
