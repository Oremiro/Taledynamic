export const emailStartsWithRegex = /^[a-zA-Z]/;
export const emailRegex = /^[\w.-]*@/;
export const emailDomainRegex = /(\.[a-zA-Z]{2,})+$/;
export const passwordRegex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_(),.+-]).{8,64}$/;

export function externalOptions(value) {
    const base = ['gmail.com', 'outlook.com', 'yandex.ru', 'mail.ru', 'aol.com', 'list.ru', 'bk.ru', 'inbox.ru'];
    let [prefix, suffix] = value.split('@');
    let filteredBase;
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