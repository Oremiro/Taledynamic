<template>
  <transition name="fade" mode="out-in">
    <div
      v-if="isInitializationSuccess"
      style="display: flex; gap: 1rem; flex-wrap: wrap"
    >
      <table-creating-item
        :workspace-id="props.workspaceId"
        :tables-count="tables.length"
      />
      <n-button v-for="table of tables" :key="table.id">{{
        table.name
      }}</n-button>
    </div>
    <tables-loading-item
      v-else
      :error="isInitializationError"
      @repeat-initialization="initializeTablesList"
    />
  </transition>
</template>

<script setup lang="ts">
import axios from "axios";
import { ref, onMounted, watch, computed } from "vue";
import { useMessage } from "naive-ui";
import { TableDto } from "@/interfaces/api/responses";
import { TableApi } from "@/helpers/api/table";
import { useStore } from "@/store";
import { debounce } from "@/helpers";
import TablesLoadingItem from "@/components/tables/TablesLoadingItem.vue";
import TableCreatingItem from "@/components/tables/TableCreatingItem.vue";
import { InitializationStatus } from "@/interfaces";

const props = defineProps<{
  workspaceId: number;
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

const isInitializationError = computed<boolean>(() => tablesInitializationStatus.value === InitializationStatus.Error);
const isInitializationSuccess = computed<boolean>(() => tablesInitializationStatus.value === InitializationStatus.Success);

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
