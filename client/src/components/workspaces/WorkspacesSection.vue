<template>
  <transition name="fade" mode="out-in">
    <workspace-loading
      v-if="isListLoading"
      :error="isListLoadingError"
      @repeat-loading="getWorkspaces"
    />
    <div v-else class="workspaces-section-content">
      <div class="workspaces-section-content-header">
        <n-text depth="3"> Ваши рабочие пространства </n-text>
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
      <n-scrollbar style="height: 100%">
        <workspaces-list :workspaces="workspaces" />
      </n-scrollbar>
      <div class="workspaces-section-content-footer">
        <workspace-creating-button />
      </div>
    </div>
  </transition>
</template>

<script setup lang="ts">
import { ref, computed, watch } from "vue";
import {
  SelectGroupOption,
  SelectOption,
  NPopselect,
  NScrollbar
} from "naive-ui";
import { useStore } from "@/store";
import {
  Workspace,
  WorkspacesSortType
} from "@/interfaces/store";
import { InitializationStatus } from "@/interfaces";
import WorkspaceCreatingButton from "@/components/workspaces/WorkspaceCreating.vue";
import WorkspacesList from "@/components/workspaces/WorkspacesList.vue";
import WorkspaceLoading from "@/components/workspaces/WorkspacesLoading.vue";
import ArrowSortIcon from "@/components/icons/ArrowSortIcon.vue";

const store = useStore();

const workspaces = computed<Workspace[]>(
  () => store.getters["workspaces/workspaces"]
);
const popOptions: Array<SelectOption | SelectGroupOption> = [
  {
    type: "group",
    label: "По дате",
    key: "sortByDate",
    children: [
      {
        label: "Сначала новые",
        value: WorkspacesSortType.DateDescending
      },
      {
        label: "Сначала старые",
        value: WorkspacesSortType.DateAscending
      }
    ]
  },
  {
    type: "group",
    label: "По названию",
    key: "sortByName",
    children: [
      {
        label: "От A до Z",
        value: WorkspacesSortType.NameAscending
      },
      {
        label: "От Z до A",
        value: WorkspacesSortType.NameDescending
      }
    ]
  }
];

const popSortValueStored: string | null =
  localStorage.getItem("workspacesSort");
const popSortValueParsed: number = popSortValueStored
  ? parseInt(popSortValueStored)
  : WorkspacesSortType.DateDescending;
const popSortValue = ref<WorkspacesSortType>(
  !isNaN(popSortValueParsed)
    ? popSortValueParsed
    : WorkspacesSortType.DateDescending
);

const isListLoading = ref<boolean>(workspaces.value.length ? false : true);
const isListLoadingError = ref<boolean>(false);

async function updateHandler(value: number): Promise<void> {
  localStorage.setItem("workspacesSort", value.toString());
  await store.dispatch("workspaces/sort", { sortType: value });
}

async function getWorkspaces(): Promise<void> {
  try {
    await store.dispatch("workspaces/init");
  } catch (error) {
    if (error instanceof Error) {
      console.log(error.message);
    }
  }
}

const workspacesInitStatus = computed<InitializationStatus>(
  () => store.getters["workspaces/initStatus"]
);
watch(workspacesInitStatus, async (value) => {
  switch (value) {
    case InitializationStatus.Success: {
      await store.dispatch("workspaces/sort", { sortType: popSortValue.value });
      isListLoading.value = false;
      break;
    }
    case InitializationStatus.Error: {
      isListLoadingError.value = true;
      break;
    }
    case InitializationStatus.Pending: {
      isListLoadingError.value = false;
      isListLoading.value = true;
      break;
    }
  }
});

watch(workspaces, async () => {
  await store.dispatch("workspaces/sort", { sortType: popSortValue.value });
});
</script>

<style lang="scss" scoped>
.workspaces-section-content {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.workspaces-section-content-header {
  padding: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.workspaces-section-content-footer {
  padding: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
</style>
