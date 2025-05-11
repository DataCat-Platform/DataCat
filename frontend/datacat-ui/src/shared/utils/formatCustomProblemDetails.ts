import {CustomProblemDetails} from "../services/datacat-generated-client";

export function formatCustomProblemDetails(problem: CustomProblemDetails): string {
    if (!problem.errors || Object.keys(problem.errors).length === 0) {
        return problem.detail ?? "An unknown error occurred.";
    }

    const lines: string[] = [];

    for (const [key, messages] of Object.entries(problem.errors)) {
        for (const message of messages) {
            lines.push(`${key}: ${message}`);
        }
    }

    return lines.join('\n');
}
