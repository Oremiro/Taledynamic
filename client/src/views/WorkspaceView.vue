<template>
  <div class="container" style="padding: 4rem 0">
    <n-card style="max-width: 40rem">
      <template #header>
        <n-page-header v-if="isInitializationSuccess">
          <template #title>
            <n-text type="success">{{ currentWorkspace?.name }}</n-text>
          </template>
          <template #subtitle>
            <div>
              Дата изменения: {{ currentWorkspace?.modified.toLocaleString() }}
            </div>
          </template>
          <template #extra>
            <div style="display: flex; gap: 0.5rem">
              <n-button text @click="isTablesEditable = !isTablesEditable">
                <n-icon size="1.2rem">
                  <edit-icon />
                </n-icon>
              </n-button>
              <n-popselect
                v-model:value="popSortValue"
                trigger="click"
                :options="popOptions"
                @update:value="updateHandler"
              >
                <n-button text>
                  <n-icon size="1.2rem">
                    <arrow-sort-icon />
                  </n-icon>
                </n-button>
              </n-popselect>
            </div>
          </template>
        </n-page-header>
        <n-skeleton v-else text round />
        <n-divider style="margin-bottom: 0" />
      </template>
      <template #default>
        <tables-list ref="tablesListRef" :workspace-id="workspaceId" :editable="isTablesEditable" :sort-type="popSortValue" />
      </template>
    </n-card>
  </div>
</template>

<script setup lang="ts">
import { computed, watch, onMounted, ref } from "vue";
import { NPageHeader, NDivider, NSkeleton, SelectOption, SelectGroupOption, NPopselect } from "naive-ui";
import { useRouter } from "vue-router";
import { useStore } from "@/store";
import { Workspace } from "@/interfaces/store";
import { InitializationStatus, TablesSortType } from "@/interfaces";
import TablesList from "@/components/tables/TablesList.vue";
import { ArrowSortIcon, EditIcon } from "@/components/icons";

const props = defineProps({
  id: {
    type: String,
    required: true
  }
});

const workspaceId = computed<number>(() => parseInt(props.id));

const store = useStore();
const workspaces = computed<Workspace[]>(
  () => store.getters["workspaces/workspaces"]
);
const currentWorkspace = computed<Workspace | null>(
  () => store.getters["workspaces/currentWorkspace"]
);

const router = useRouter();

async function setCurrentWorkspace(id: number): Promise<void> {
  if (isNaN(id)) {
    router.push({ name: "NotFound" });
    return;
  }
  const currentWorkspace: Workspace | undefined = workspaces.value.find(
    (item) => item.id === id
  );
  if (currentWorkspace) {
    store.commit("workspaces/setCurrent", { workspace: currentWorkspace });
  } else {
    router.push({ name: "NotFound" });
  }
}

const isTablesEditable = ref<boolean>(false);

const popOptions: Array<SelectOption | SelectGroupOption> = [
  {
    type: "group",
    label: "По названию",
    key: "sortByName",
    children: [
      {
        label: "От A до Z",
        value: TablesSortType.NameAscending
      },
      {
        label: "От Z до A",
        value: TablesSortType.NameDescending
      }
    ]
  }
];
const popSortValueStored: string | null =
  localStorage.getItem("tablesSort");
const popSortValueParsed: number = popSortValueStored
  ? parseInt(popSortValueStored)
  : TablesSortType.NameAscending;
const popSortValue = ref<TablesSortType>(
  !isNaN(popSortValueParsed)
    ? popSortValueParsed
    : TablesSortType.NameDescending
);

const tablesListRef = ref<InstanceType<typeof TablesList>>();

async function updateHandler(value: number): Promise<void> {
  localStorage.setItem("tablesSort", value.toString());
  // @ts-expect-error: vue-next #4397
  await tablesListRef.value?.sortList(popSortValue.value);
}

const isInitializationSuccess = computed<boolean>(
  () => workspacesInitStatus.value === InitializationStatus.Success
);

/* Setting current workspace */
const workspacesInitStatus = computed<InitializationStatus>(
  () => store.getters["workspaces/initStatus"]
);
onMounted(async () => {
  if (isInitializationSuccess.value) {
    await setCurrentWorkspace(workspaceId.value);
  }
});
watch(workspacesInitStatus, async (value) => {
  if (value === InitializationStatus.Success) {
    await setCurrentWorkspace(workspaceId.value);
  }
});
watch(workspaceId, async (value) => {
  await setCurrentWorkspace(value);
});
</script>
