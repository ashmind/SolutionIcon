using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;

namespace SolutionIcon.Implementation {
    public class RecursiveSolutionWalker : IEnumerator<object>, IEnumerable<object> {
        private readonly Solution _solution;
        private bool _enumerated;
        private IEnumerator _enumerator;
        private readonly Stack<IEnumerator> _stack = new Stack<IEnumerator>();
        private bool _disposed;

        public RecursiveSolutionWalker(Solution solution) {
            _solution = solution;
            _enumerator = solution.Projects.GetEnumerator();
        }

        public bool MoveNext() {
            var moved = _enumerator.MoveNext();
            if (!moved) {
                DisposeEnumerator(_enumerator);
                if (_stack.Count == 0)
                    return false;

                MoveOut();
                return MoveNext();
            }

            var current = _enumerator.Current;
            Current = current;

            var project = current as Project;
            if (project != null) {
                MoveInto(project.ProjectItems);
                return true;
            }

            var projectItem = current as ProjectItem;
            if (projectItem != null) {
                if (projectItem.ProjectItems != null) {
                    MoveInto(projectItem.ProjectItems);
                }

                var subproject = projectItem.SubProject;
                if (subproject != null) {
                    MoveInto(new[] { subproject });
                }

                return true;
            }

            return true;
        }

        private void MoveInto(IEnumerable enumerable) {
            _stack.Push(_enumerator);
            _enumerator = enumerable.GetEnumerator();
        }

        private void MoveOut() {
            _enumerator = _stack.Pop();
        }

        public object Current { get; private set; }

        object IEnumerator.Current {
            get { return Current; }
        }

        public void Reset() {
            throw new NotSupportedException();
        }

        private void DisposeEnumerator(IEnumerator enumerator) {
            var disposable = enumerator as IDisposable;
            if (disposable != null)
                disposable.Dispose();
        }

        public void Dispose() {
            if (_disposed)
                return;

            var exceptions = new List<Exception>();
            _disposed = true;
            while (_stack.Count > 0) {
                try {
                    DisposeEnumerator(_stack.Pop());
                }
                catch (Exception ex) {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Count > 0)
                throw new AggregateException(exceptions[0].Message, exceptions);
        }

        public IEnumerator<object> GetEnumerator() {
            if (!_enumerated) {
                _enumerated = true;
                return this;
            }

            return new RecursiveSolutionWalker(_solution);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
