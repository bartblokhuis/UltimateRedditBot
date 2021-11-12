export interface Result<Type> {
    data: Type,
    messages: string[],
    succeeded: boolean
};