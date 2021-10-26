<template>
	<n-button-group>
		<n-button
			:attr-type="attrType"
			ghost
			@click="$emit('click', $event)"
			:loading="loading"
			:disabled="loading || (disablingDuration != 0)"
			:type="type">
			{{ text }}
		</n-button>
		<n-button v-if="disablingDuration" disabled :type="type" ghost>{{ disablingDuration }}</n-button>
	</n-button-group>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'

export default defineComponent({
	name: 'DelayedButton',
	emits: ['click'],
	props: {
		attrType: {
			type: String,
			default: 'button',
			validator: (value: string) => ['button', 'submit', 'reset'].includes(value)
		},
		text: {
			type: String,
			required: true
		},
		loading: {
			type: Boolean,
			default: false
		},
		type: {
			type: String,
			default: 'default',
			validator: (value: string) => ['default', 'primary', 'success', 'info', 'warning', 'error'].includes(value)
		},
		duration: {
			type: Number,
			default: 15
		}
	},
	setup(props) {
		const disablingDuration = ref<number>(0);

		let disablingTimer: number;
		const holdDisabled = (): void => {
			disablingDuration.value = props.duration;
			disablingTimer = setInterval(() => {
				disablingDuration.value--;
				if (disablingDuration.value === 0) {
					clearInterval(disablingTimer);
				}
			}, 1000);
		}

		const cancelHolding = (): void => {
			clearInterval(disablingTimer);
			disablingDuration.value = 0;
		}
		return {
			disablingDuration,
			holdDisabled,
			cancelHolding
		}
	},
})
</script>
