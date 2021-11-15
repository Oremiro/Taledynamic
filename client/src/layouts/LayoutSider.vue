<template>
  <transition name="fade">
    <n-layout-sider
      v-if="isLoggedIn"
      bordered
      collapse-mode="transform"
      show-trigger="bar"
      :collapsed-width="0"
      :default-collapsed="isDefaultCollapsed"
      @update:collapsed="collapsedHandler"
    >
      <workspaces-section v-if="isWorkspaceListShown" />
    </n-layout-sider>
  </transition>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useStore } from '@/store';
import WorkspacesSection from "@/components/workspaces/WorkspacesSection.vue";

const store = useStore();
const isLoggedIn = computed<boolean>(() => store.getters['user/isLoggedIn']);
const isDefaultCollapsed = ref<boolean>(localStorage.getItem('siderCollapsed') ? true : false);
const isWorkspaceListShown = ref<boolean>(!isDefaultCollapsed.value);

function collapsedHandler(collapsed: boolean): void {
	if (collapsed) {
		localStorage.setItem('siderCollapsed', '1');
	} else {
		localStorage.removeItem('siderCollapsed');
	}
	if (!isWorkspaceListShown.value) {
		isWorkspaceListShown.value = true;
	}
}
</script>