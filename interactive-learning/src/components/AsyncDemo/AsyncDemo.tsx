import React, { useState } from 'react';
import { asyncDemoService } from '../../services/apiService';
import type { 
  ThreadInfo, 
  ComparisonResult, 
  ParallelTasksResult, 
  TaskLifecycleResult,
  ThreadPoolResult,
  ConfigureAwaitResult,
  CancellationResult 
  , DetailedComparisonResult
} from '../../services/apiService';
import LoadingSpinner from '../Common/LoadingSpinner';
// import './AsyncDemo.css';

interface DemoState {
  threadInfo: ThreadInfo | null;
  comparison: ComparisonResult | null;
  parallelTasks: ParallelTasksResult | null;
  taskLifecycle: TaskLifecycleResult | null;
  threadPool: ThreadPoolResult | null;
  configureAwait: ConfigureAwaitResult | null;
  cancellation: CancellationResult | null;
  detailedComparison: DetailedComparisonResult | null;
  loading: Record<string, boolean>;
}

const AsyncDemo: React.FC = () => {
  const [state, setState] = useState<DemoState>({
    threadInfo: null,
    comparison: null,
    parallelTasks: null,
    taskLifecycle: null,
    threadPool: null,
    configureAwait: null,
    cancellation: null,
    detailedComparison: null,
    loading: {}
  });

  

  const setLoading = (key: string, value: boolean) => {
    setState(prev => ({
      ...prev,
      loading: { ...prev.loading, [key]: value }
    }));
  };

  const handleThreadInfo = async () => {
    setLoading('threadInfo', true);
    try {
      const response = await asyncDemoService.getThreadInfo();
      setState(prev => ({ ...prev, threadInfo: response.data }));
    } catch (error) {
      console.error('Thread info error:', error);
    } finally {
      setLoading('threadInfo', false);
    }
  };

  const handleSyncVsAsync = async () => {
    setLoading('comparison', true);
    try {
      const response = await asyncDemoService.compareSyncVsAsync();
      setState(prev => ({ ...prev, comparison: response.data }));
    } catch (error) {
      console.error('Sync vs Async error:', error);
    } finally {
      setLoading('comparison', false);
    }
  };

  const handleComprehensive = async () => {
    setLoading('comprehensive', true);
    try {
      await Promise.all([
        handleThreadInfo(),
        handleSyncVsAsync(),
        handleDetailedComparison(),
      ]);
    } finally {
      setLoading('comprehensive', false);
    }
  };

  const handleDetailedComparison = async () => {
    setLoading('detailedComparison', true);
    try {
      const response = await asyncDemoService.getDetailedComparison();
      setState(prev => ({ ...prev, detailedComparison: response.data }));
    } catch (error) {
      console.error('Detailed comparison error:', error);
    } finally {
      setLoading('detailedComparison', false);
    }
  };

  return (
    <div className="async-demo">
      <header className="page-header">
        <h1>🔧 Zaawansowana Demonstracja Asynchroniczności</h1>
        <p>Kompleksowe przykłady threads, tasks i async/await w .NET</p>
      </header>

      <div className="demo-section">
        <div className="demo-card full-width comprehensive-card">
          <h3>🎯 Kompleksowa Demonstracja</h3>
          <p>Uruchom demonstracje asynchroniczności</p>
          <button 
            onClick={handleComprehensive}
            disabled={state.loading.comprehensive}
            className="demo-button comprehensive-button"
          >
            {state.loading.comprehensive ? <LoadingSpinner /> : 'Uruchom Demo'}
          </button>
        </div>
      </div>

      <div className="demo-section">
        <div className="demo-card">
          <h3>🧵 Informacje o Wątkach</h3>
          <p>Sprawdź jak await wpływa na wątki</p>
          <button 
            onClick={handleThreadInfo}
            disabled={state.loading.threadInfo}
            className="demo-button thread-button"
          >
            {state.loading.threadInfo ? <LoadingSpinner /> : 'Sprawdź Wątki'}
          </button>
          {state.threadInfo && (
            <div className="result-display">
              <div className="thread-info">
                <div className="thread-item">
                  <span className="label">Przed await:</span>
                  <span className="value">Thread {state.threadInfo.threadBeforeAwait}</span>
                </div>
                <div className="thread-item">
                  <span className="label">Po await:</span>
                  <span className="value">Thread {state.threadInfo.threadAfterAwait}</span>
                </div>
                <div className="thread-item">
                  <span className="label">ThreadPool:</span>
                  <span className={`value ${state.threadInfo.isThreadPoolThread ? 'yes' : 'no'}`}>
                    {state.threadInfo.isThreadPoolThread ? 'Tak' : 'Nie'}
                  </span>
                </div>
                <div className="explanation">
                  💡 {state.threadInfo.explanation}
                </div>
              </div>
            </div>
          )}
        </div>

        <div className="demo-card">
          <h3>⚡ Sync vs Async</h3>
          <p>Porównanie wydajności</p>
          <button 
            onClick={handleSyncVsAsync}
            disabled={state.loading.comparison}
            className="demo-button comparison-button"
          >
            {state.loading.comparison ? <LoadingSpinner /> : 'Porównaj'}
          </button>
          <button
            onClick={handleDetailedComparison}
            disabled={state.loading.detailedComparison}
            className="demo-button detailed-button"
            style={{ marginLeft: 12 }}
          >
            {state.loading.detailedComparison ? <LoadingSpinner /> : 'Pokaż szczegóły'}
          </button>
          {state.comparison && (
            <div className="result-display">
              <div className="performance-bars">
                <div className="performance-item">
                  <span>Synchroniczne:</span>
                  <div className="bar sync-bar">
                    <span>{state.comparison.synchronousTime}ms</span>
                  </div>
                </div>
                <div className="performance-item">
                  <span>Asynchroniczne:</span>
                  <div className="bar async-bar">
                    <span>{state.comparison.asynchronousTime}ms</span>
                  </div>
                </div>
              </div>
              <div className="explanation">
                💡 {state.comparison.explanation}
              </div>
            </div>
          )}

          {state.detailedComparison && (
            <div className="result-display detailed-result">
              <h4>🔎 Szczegółowe porównanie</h4>
              <div className="env-snapshot">
                <strong>Environment:</strong>
                <div>Processors: {state.detailedComparison.environmentSnapshot.processorCount}</div>
                <div>ProcessId: {state.detailedComparison.environmentSnapshot.processId}</div>
                <div>ThreadCount: {state.detailedComparison.environmentSnapshot.threadCount}</div>
              </div>

              <div className="threadpool-snapshots">
                <strong>ThreadPool (before):</strong>
                <div>Worker available: {state.detailedComparison.threadPoolBefore.workerThreadsAvailable}</div>
                <div>Completion available: {state.detailedComparison.threadPoolBefore.completionPortThreadsAvailable}</div>
                <div>Max worker: {state.detailedComparison.threadPoolBefore.workerThreadsMax}</div>
                <div>Max completion: {state.detailedComparison.threadPoolBefore.completionPortThreadsMax}</div>

                <strong style={{ marginTop: 8 }}>After Sync:</strong>
                <div>Worker available: {state.detailedComparison.threadPoolAfterSync.workerThreadsAvailable}</div>
                <div>Completion available: {state.detailedComparison.threadPoolAfterSync.completionPortThreadsAvailable}</div>

                <strong style={{ marginTop: 8 }}>After Async:</strong>
                <div>Worker available: {state.detailedComparison.threadPoolAfterAsync.workerThreadsAvailable}</div>
                <div>Completion available: {state.detailedComparison.threadPoolAfterAsync.completionPortThreadsAvailable}</div>
              </div>

              <div className="summary-list">
                <strong>Summary:</strong>
                <ul>
                  {state.detailedComparison.summary.map((s, i) => (
                    <li key={i}>{s}</li>
                  ))}
                </ul>
              </div>

              <div className="operations-compare">
                <div>
                  <strong>Sync group:</strong>
                  <div>Total: {state.detailedComparison.synchronous.totalElapsedMs} ms</div>
                  <div>Distinct threads: {state.detailedComparison.synchronous.distinctThreadIds.length}</div>
                  <details>
                    <summary>Operations (sync)</summary>
                    <ol>
                      {state.detailedComparison.synchronous.operations.map(op => (
                        <li key={op.index}>
                          #{op.index} — {op.elapsedMs}ms — startThread:{op.startThreadId} endThread:{op.endThreadId} — {new Date(op.startTimeUtc).toLocaleTimeString()} → {new Date(op.endTimeUtc).toLocaleTimeString()}
                        </li>
                      ))}
                    </ol>
                  </details>
                </div>

                <div style={{ marginTop: 8 }}>
                  <strong>Async group:</strong>
                  <div>Total: {state.detailedComparison.asynchronous.totalElapsedMs} ms</div>
                  <div>Distinct threads: {state.detailedComparison.asynchronous.distinctThreadIds.length}</div>
                  <details>
                    <summary>Operations (async)</summary>
                    <ol>
                      {state.detailedComparison.asynchronous.operations.map(op => (
                        <li key={op.index}>
                          #{op.index} — {op.elapsedMs}ms — startThread:{op.startThreadId} endThread:{op.endThreadId} — {new Date(op.startTimeUtc).toLocaleTimeString()} → {new Date(op.endTimeUtc).toLocaleTimeString()}
                        </li>
                      ))}
                    </ol>
                  </details>
                </div>
              </div>
            </div>
          )}
        </div>
      </div>
    </div>
  );
};

export default AsyncDemo;
