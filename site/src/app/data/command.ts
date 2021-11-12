export interface CommandGroup {
    name: string
    commands: Command[];
}

export interface Command {
    name: string,
    command: string,
    description: string,
    usage: string,
    modOnly: boolean,
    ownerOnly: boolean
}