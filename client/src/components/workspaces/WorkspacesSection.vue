<template>
  <n-card
    style="height: calc(100vh - 8.3rem)"
    content-style="height: 100%;"
  >
    <transition name="fade" mode="out-in">
      <div v-if="isInitializationSuccess && workspacesLength > 0" class="workspaces-section-content">
        <div class="workspaces-section-content-header">
          <n-text depth="3"> Ваши рабочие пространства </n-text>
          <workspaces-sort-item />
        </div>
        <n-scrollbar style="height: 100%">
          <workspaces-list />
        </n-scrollbar>
        <div class="workspaces-section-content-footer">
          <workspace-creating-item />
        </div>
      </div>
      <div v-else-if="isInitializationSuccess && workspacesLength <= 0" class="workspaces-section-empty">
        <n-empty size="large" description="Рабочих пространств нет" />
        <workspace-creating-item />
      </div>
      <workspace-loading
        v-else-if="!isInitializationSuccess"
        :error="isInitializationError"
        @repeat-loading="initWorkspaces"
      />
    </transition>
  </n-card>
</template>

<script setup lang="ts">
import { computed } from "vue";
import { NScrollbar, NEmpty } from "naive-ui";
import { useStore } from "@/store";
import { InitializationStatus } from "@/models";
import WorkspaceCreatingItem from "@/components/workspaces/WorkspaceCreatingItem.vue";
import WorkspacesList from "@/components/workspaces/WorkspacesList.vue";
import WorkspaceLoading from "@/components/workspaces/WorkspacesLoading.vue";
import WorkspacesSortItem from "@/components/workspaces/WorkspacesSortItem.vue";

const store = useStore();

const workspacesLength = computed<number>(() => store.getters["workspaces/workspacesLength"]);

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
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  height: 100%;
}

.workspaces-section-content-header {
  padding-bottom: 1rem;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 1rem;
}
.workspaces-section-content-footer {
  padding-top: 1rem;
  display: flex;
  align-items: center;
  justify-content: center;
}
.workspaces-section-empty {
  height: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 1rem;
}
</style>
