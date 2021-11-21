<template>
  <transition name="fade" mode="out-in">
    <div v-if="isInitializationSuccess && tables.length">
      <div
        style="
          display: flex;
          align-items: center;
          justify-content: space-between;
          margin-bottom: 1.5rem;
        "
      >
        <n-text>Ваши таблицы</n-text>
        <n-tag>{{ tables.length }} / 100</n-tag>
      </div>
      <div style="display: flex; gap: 1rem; flex-wrap: wrap">
        <tables-list-item
          v-for="table of tables"
          :id="table.id"
          :key="table.id"
          :name="table.name"
          :editable="editable"
          @update="updateListItem"
          @delete="deleteListItem"
        />
        <table-creating-item
          v-if="tables.length < 100"
          :workspace-id="props.workspaceId"
          :tables-count="tables.length"
          type="primary"
          @create="pushTableToList"
        >
          <n-icon size="1.2rem">
            <add-icon />
          </n-icon>
        </table-creating-item>
      </div>
    </div>
    <n-empty
      v-else-if="isInitializationSuccess && !tables.length"
      size="large"
      description="Здесь пока что нет таблиц"
      style="height: 15rem; display: flex; justify-content: center"
    >
      <template #extra>
        <table-creating-item
          :workspace-id="props.workspaceId"
          :tables-count="tables.length"
          @create="pushTableToList"
        >
          Создать таблицу
        </table-creating-item>
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
import { ref, onMounted, watch, computed } from "vue";
import { useMessage, NEmpty, NTag } from "naive-ui";
import { TableDto } from "@/interfaces/api/responses";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { debounce } from "@/helpers";
import { InitializationStatus } from "@/interfaces";
import TablesLoadingItem from "@/components/tables/TablesLoadingItem.vue";
import TableCreatingItem from "@/components/tables/TableCreatingItem.vue";
import TablesListItem from "@/components/tables/TablesListItem.vue";
import { AddIcon } from "@/components/icons";

const props = defineProps<{
  workspaceId: number;
  editable?: boolean;
}>();

const tables = ref<TableDto[]>([]);

const store = useStore();
const message = useMessage();

const tablesInitializationStatus = ref<InitializationStatus>(
  InitializationStatus.Pending
);

async function initializeTablesList(): Promise<void> {
  tablesInitializationStatus.value = InitializationStatus.Pending;
  try {
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
    // TODO: 401 Handler
    if (axios.isAxiosError(error)) {
      message.error(error.message);
    }
  }
}

function pushTableToList(table: TableDto) {
  tables.value.push(table);
}

function updateListItem(oldId: number, table: TableDto) {
  const indexFound = tables.value.findIndex((item) => item.id === oldId);
  if (~indexFound) {
    tables.value[indexFound] = table;
  }
}

function deleteListItem(id: number) {
  const indexFound = tables.value.findIndex((item) => item.id === id);
  if (~indexFound) {
    tables.value.splice(indexFound, 1);
  }
}

const isInitializationError = computed<boolean>(
  () => tablesInitializationStatus.value === InitializationStatus.Error
);
const isInitializationSuccess = computed<boolean>(
  () => tablesInitializationStatus.value === InitializationStatus.Success
);

const initializeTableListDebounced = debounce(initializeTablesList, 1000);
onMounted(async () => {
  await initializeTableListDebounced();
});

watch(
  () => props.workspaceId,
  async () => {
    tablesInitializationStatus.value = InitializationStatus.Pending;
    await initializeTableListDebounced();
  }
);
</script>
