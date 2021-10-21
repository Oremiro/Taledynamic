import { AutoCompleteOption } from "naive-ui";
import { Ref } from "vue";

export const emailRegex = /^[a-zA-Z0-9][\w.-]*@[a-zA-Z]{2,}(\.[a-zA-Z]{2,})+$/;
export const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_(),.+-]).{8,64}$/;


export function externalOptions(value: string): AutoCompleteOption[] {
    const base: string[] = ['gmail.com', 'outlook.com', 'yandex.ru', 'mail.ru', 'aol.com', 'list.ru', 'bk.ru', 'inbox.ru'];
    const [prefix, suffix]: string[] = value.split('@');
    let filteredBase: string[];
    if (suffix !== '') {
        filteredBase = base.filter(item => item.startsWith(suffix)).slice(0, 3);
    } else {
        filteredBase = base.slice(0, 3);
    }
    return filteredBase.map((suffix) => {
        return {
            label: `${prefix}@${suffix}`,
            value: `${prefix}@${suffix}`
        }
    })
}

export function holdSubmitDisabled(submitDisabled: Ref<number>): void {
	submitDisabled.value = 15;
	const submitDisabledTimer = setInterval(() => {
		submitDisabled.value--;
		if (submitDisabled.value == 0) {
			clearInterval(submitDisabledTimer);
		}
	}, 1000);
}