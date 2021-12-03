<template>
  <transition name="fade" mode="out-in">
    <div v-if="isInitializationSuccess" class="workspaces-section-content">
      <transition name="fade" mode="out-in">
        <div v-if="workspaces.length" style="height: 100%">
          <div class="workspaces-section-content-header">
            <n-text depth="3"> Ваши рабочие пространства </n-text>
            <workspaces-sort-item />
          </div>
          <n-scrollbar style="height: 100%">
            <workspaces-list :workspaces="workspaces" />
          </n-scrollbar>
        </div>
        <n-empty v-else size="large" description="Рабочих пространств нет" class="workspaces-section-content-empty" />
      </transition>
      <div class="workspaces-section-content-footer">
        <workspace-creating-button />
      </div>
    </div>
    <workspace-loading v-else :error="isInitializationError" @repeat-loading="initWorkspaces" />
  </transition>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { NScrollbar, NEmpty } from "naive-ui";
import { useStore } from "@/store";
import { Workspace } from "@/models/store";
import { InitializationStatus } from "@/models";
import WorkspaceCreatingButton from "@/components/workspaces/WorkspaceCreating.vue";
import WorkspacesList from "@/components/workspaces/WorkspacesList.vue";
import WorkspaceLoading from "@/components/workspaces/WorkspacesLoading.vue";
import WorkspacesSortItem from "@/components/workspaces/WorkspacesSortItem.vue";

const store = useStore();

const workspaces = computed<Workspace[]>(() => store.getters["workspaces/workspaces"]);
const isInitializationSuccess = computed<boolean>(
  () => store.getters["workspaces/initStatus"] === InitializationStatus.Success
);
const isInitializationError = computed<boolean>(
  () => store.getters["workspaces/initStatus"] === InitializationStatus.Error
);

async function initWorkspaces(): Promise<void> {
  try {
    await store.dispatch("workspaces/init");
  } catch (error) {
    if (error instanceof Error) {
      console.log(error.message);
    }
  }
}
</script>

<style lang="scss" scoped>
.workspaces-section-content {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.workspaces-section-content-header {
  // padding: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.workspaces-section-content-footer {
  // padding: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
.workspaces-section-content-empty {
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>
