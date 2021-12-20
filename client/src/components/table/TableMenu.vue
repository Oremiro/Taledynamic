<template>
  <n-page-header style="padding: 0 0.35rem" @back="backToMain">
    <template #title>
      <div style="display: flex; align-items: center">
        <n-spin v-if="savingStatus === 0" :size="22">
          <template #icon>
            <n-icon>
              <arrow-sync-icon />
            </n-icon>
          </template>
        </n-spin>
        <n-tooltip v-else :show-arrow="false">
          <template #default> Последнее сохранение: {{ lastSavingDateTime.toLocaleString() }} </template>
          <template #trigger>
            <n-button :type="savingStatus == 2 ? 'error' : 'default'" text @click="pushTable">
              <n-icon size="1.4rem">
                <save-icon />
              </n-icon>
            </n-button>
          </template>
        </n-tooltip>
        <n-menu :value="currentTable?.id" mode="horizontal" :options="tablesListMenu" />
      </div>
    </template>
  </n-page-header>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, h, watch, onUnmounted } from "vue";
import { RouterLink, useRouter } from "vue-router";
import { NPageHeader, useMessage, MenuOption, NSpin, NEllipsis } from "naive-ui";
import { TableApi } from "@/helpers/api/table";
import { TableDto } from "@/models/api/responses";
import { useStore } from "@/store";
import { SaveIcon, ArrowSyncIcon } from "@/components/icons";
import { InitializationStatus } from "@/models";
import { debounce } from "@/helpers";

const props = defineProps<{
  workspaceId: number;
  tableId: number;
}>();

const tablesList = ref<TableDto[]>([]);

const tablesListMenu = computed<MenuOption[]>(() =>
  tablesList.value.map((item) => ({
    label: () =>
      h(
        NEllipsis,
        {
          style: {
            maxWidth: "10rem"
          }
        },
        {
          default: () =>
            h(
              RouterLink,
              {
                to: {
                  name: "Table",
                  params: {
                    workspaceId: props.workspaceId,
                    tableId: item.id
                  }
                }
              },
              {
                default: () => item.name
              }
            ),
          tooltip: () => item.name
        }
      ),
    key: item.id
  }))
);

const currentTable = computed<TableDto | undefined>(() => tablesList.value.find((item) => item.id === props.tableId));

const store = useStore();
const message = useMessage();
async function getTablesList(): Promise<void> {
  try {
    await store.dispatch("user/refreshExpired");
    const { data } = await TableApi.getList({ workspaceId: props.workspaceId }, store.getters["user/accessToken"]);
    tablesList.value = data.tables;
  } catch (error) {
    if (error instanceof Error) {
      message.error(error.message);
    }
  }
}

const router = useRouter();
function backToMain() {
  router.push({ name: "Main" });
}

const savingStatus = ref<InitializationStatus>(InitializationStatus.Success);

const pushTable = debounce(
  async () => {
    try {
      await store.dispatch("table/pushTable");
      savingStatus.value = InitializationStatus.Success;
      lastSavingDateTime.value = new Date(Date.now());
      message.success("Изменения сохранены");
    } catch (error) {
      savingStatus.value = InitializationStatus.Error;
      if (error instanceof Error) {
        message.error("При сохранении произошла ошибка");
      }
    }
  },
  1000,
  {
    immediateFunc: () => {
      savingStatus.value = InitializationStatus.Pending;
    }
  }
);

onMounted(async () => {
  await getTablesList();
});

const autoSavingTimer = ref<ReturnType<typeof setInterval>>();

const lastSavingDateTime = ref<Date>(new Date(Date.now()));

watch(
  () => props.tableId,
  () => {
    if (autoSavingTimer.value !== undefined) {
      clearInterval(autoSavingTimer.value);
    }
    autoSavingTimer.value = setInterval(async () => {
      await pushTable();
    }, 120000);
  },
  { immediate: true }
);

onUnmounted(() => {
  if (autoSavingTimer.value !== undefined) {
    clearInterval(autoSavingTimer.value);
  }
});
</script>
