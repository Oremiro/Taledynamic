import { AutoCompleteOption } from "naive-ui";

export const emailRegex = /^[a-zA-Z0-9][\w.-]*@[a-zA-Z]{2,}(\.[a-zA-Z]{2,})+$/;
export const passwordRegex =
  /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[#?!@$%^&*_(),.+-]).{8,64}$/;

export function externalOptions(value: string): AutoCompleteOption[] {
  const base: string[] = [
    "gmail.com",
    "outlook.com",
    "yandex.ru",
    "mail.ru",
    "aol.com",
    "list.ru",
    "bk.ru",
    "inbox.ru",
  ];
  const [prefix, suffix]: string[] = value.split("@");
  let filteredBase: string[];
  if (suffix !== "") {
    filteredBase = base.filter((item) => item.startsWith(suffix)).slice(0, 3);
  } else {
    filteredBase = base.slice(0, 3);
  }
  return filteredBase.map((suffix) => {
    return {
      label: `${prefix}@${suffix}`,
      value: `${prefix}@${suffix}`,
    };
  });
}

export async function workspaceNameValidator(
  targetValue: string
): Promise<void> {
  const trimmedValue: string = targetValue.trim();
  if (trimmedValue === "") {
    throw new Error("Required");
  } else if (trimmedValue !== targetValue) {
    throw new Error("Starts/ends with whitespaces");
  } else {
    return;
  }
}

export function debounce<A extends unknown[], R>(
  fn: (...args: A) => R | Promise<R>,
  ms = 500,
  options: {
    isAwaited?: boolean;
    immediateFunc?: () => void;
  } = {}
): (...args: A) => Promise<R> {
  let timeoutId: ReturnType<typeof setTimeout> | undefined;
  return function (this: ThisParameterType<typeof fn>, ...args: A): Promise<R> {
    return new Promise<R>((resolve, reject) => {
      if (options.immediateFunc !== undefined) {
        options.immediateFunc();
      }
      if (timeoutId !== undefined) {
        clearTimeout(timeoutId);
      }
      timeoutId = setTimeout(async () => {
        try {
          const result = fn.apply(this, args);
          if (options.isAwaited) {
            resolve(await result);
          } else {
            resolve(result);
          }
        } catch (error) {
          reject(error);
        }
      }, ms);
    });
  };
}
