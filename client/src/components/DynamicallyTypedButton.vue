<template>
	<n-button 
		@mouseenter="isHovered = true" 
		@mouseleave="isHovered = false"
		@focusin="isFocused=true"
		@focusout="isFocused=false"
		:type="isHovered || isFocused ? type : 'default'">
		<slot></slot>
		<template #icon v-if="$slots.icon">
			<slot name="icon"></slot>
		</template>
	</n-button>
</template>
<script setup lang="ts">
import { ref, PropType } from 'vue';

/* global defineProps */
defineProps({
	type: {
		required: true,
		type: String as PropType<'success' | 'info' | 'warning' | 'error'>
	}
})

const isHovered = ref<boolean>(false)
const isFocused = ref<boolean>(false);
</script>