﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ploeh.AutoFixture.Kernel
{
    /// <summary>
    /// Relays requests for many (an unspecified count) specimens to a request for a specific
    /// number of specimens.
    /// </summary>
    public class ManyRelay : ISpecimenBuilder, IMany
    {
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManyRelay"/> class.
        /// </summary>
        public ManyRelay()
        {
            this.count = 3;
        }

        #region IMany Members

        /// <summary>
        /// Gets or sets the count that specifies how many specimens will be requested.
        /// </summary>
        public int Count
        {
            get { return this.count; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Count cannot be zero or negative.");
                }
                this.count = value;
            }
        }

        #endregion

        #region ISpecimenBuilder Members

        /// <summary>
        /// Creates many new specimens based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        /// The requested specimens if possible; otherwise a <see cref="NoSpecimen"/> instance.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The number of specimens requested is determined by <see cref="Count"/>.
        /// </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var manyRequest = request as ManyRequest;
            if (manyRequest == null)
            {
                return new NoSpecimen(request);
            }

            return context.Resolve(new FiniteSequenceRequest(manyRequest.Request, this.Count));
        }

        #endregion
    }
}