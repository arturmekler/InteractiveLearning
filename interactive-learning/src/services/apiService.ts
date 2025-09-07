import axios, { AxiosResponse } from 'axios';

const API_BASE_URL = 'http://localhost:5096/api';

// Dodaj nowe interfejsy dla AsyncDemo
export interface ThreadInfo {
  threadBeforeAwait: number;
  threadAfterAwait: number;
  isThreadPoolThread: boolean;
  threadName: string;
  explanation: string;
}

export interface ComparisonResult {
  synchronousTime: number;
  asynchronousTime: number;
  totalTime: number;
  explanation: string;
}

export interface TaskResult {
  taskId: number;
  startThreadId: number;
  endThreadId: number;
  executionTime: number;
  startTime: string;
  endTime: string;
  threadSwitched: boolean;
}

export interface ParallelTasksResult {
  tasks: TaskResult[];
  totalExecutionTime: number;
  taskCount: number;
  explanation: string;
}

export interface LifecycleStep {
  step: string;
  status: string;
  threadId: number;
  timestamp: string;
}

export interface TaskLifecycleResult {
  result: string;
  lifecycleSteps: LifecycleStep[];
  explanation: string;
}

export interface ThreadPoolInfo {
  taskId: number;
  threadId: number;
  isThreadPoolThread: boolean;
  startTime: string;
  isCompletion: boolean;
}

export interface ThreadPoolResult {
  threadInfos: ThreadPoolInfo[];
  availableWorkerThreads: number;
  availableCompletionPortThreads: number;
  maxWorkerThreads: number;
  maxCompletionPortThreads: number;
  uniqueThreadsUsed: number;
  explanation: string;
}

export interface ConfigureAwaitStep {
  description: string;
  threadBefore: number;
  threadAfter: number;
  contextCaptured: boolean;
}

export interface ConfigureAwaitResult {
  steps: ConfigureAwaitStep[];
  explanation: string;
}

export interface StreamItem {
  id: number;
  data: string;
  threadId: number;
  timestamp: string;
  explanation: string;
}

export interface CancellationResult {
  completedSteps: string[];
  wasCancelled: boolean;
  explanation: string;
}

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Istniejące serwisy...
export const asyncBasicsService = {
  getSynchronous: (): Promise<AxiosResponse<any>> => 
    apiClient.get('/AsyncBasics/synchronous'),
  getAsynchronous: (): Promise<AxiosResponse<any>> => 
    apiClient.get('/AsyncBasics/asynchronous'),
  getParallelTasks: (): Promise<AxiosResponse<any>> => 
    apiClient.get('/AsyncBasics/parallel-tasks'),
};

// NOWY AsyncDemo serwis
export const asyncDemoService = {
  getThreadInfo: (): Promise<AxiosResponse<ThreadInfo>> =>
    apiClient.get('/AsyncDemo/thread-info'),
  compareSyncVsAsync: (): Promise<AxiosResponse<ComparisonResult>> =>
    apiClient.get('/AsyncDemo/sync-vs-async'),
  getParallelTasks: (): Promise<AxiosResponse<ParallelTasksResult>> =>
    apiClient.get('/AsyncDemo/parallel-tasks'),
  getTaskLifecycle: (): Promise<AxiosResponse<TaskLifecycleResult>> =>
    apiClient.get('/AsyncDemo/task-lifecycle'),
  getThreadPool: (): Promise<AxiosResponse<ThreadPoolResult>> =>
    apiClient.get('/AsyncDemo/thread-pool'),
  getConfigureAwait: (): Promise<AxiosResponse<ConfigureAwaitResult>> =>
    apiClient.get('/AsyncDemo/configure-await'),
  getAsyncStream: (): Promise<AxiosResponse<StreamItem[]>> =>
    apiClient.get('/AsyncDemo/async-stream'),
  getCancellationDemo: (signal?: AbortSignal): Promise<AxiosResponse<CancellationResult>> =>
    apiClient.get('/AsyncDemo/cancellation-demo', { signal }),
  getComprehensiveDemo: (): Promise<AxiosResponse<any>> =>
    apiClient.get('/AsyncDemo/comprehensive-demo'),
};
