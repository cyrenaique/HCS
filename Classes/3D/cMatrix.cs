// ----------------------------------------------
// Lutz Roeder's Mapack for .NET, September 2000
// Adapted from Mapack for COM and Jama routines.
// http://www.aisto.com/roeder/dotnet
// ----------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.IO;
using System.Globalization;

namespace HCSAnalyzer.Classes._3D
{
    public class Matrix : IEquatable<Matrix>
    {

               #region Fields
        private double[][] data;
		private int rows;
		private int columns;

		private static Random random = new Random();
        #endregion // Fields

        #region Constructors
        /// <summary>Constructs an empty matrix of the given size.</summary>
		/// <param name="rows">Number of rows.</param>
		/// <param name="columns">Number of columns.</param>
		public Matrix(int rows, int columns)
		{
			this.rows = rows;
			this.columns = columns;
			this.data = new double[rows][];
			for (int i = 0; i < rows; i++)
			{
				this.data[i] = new double[columns];
			}
		}
	
		/// <summary>Constructs a matrix of the given size and assigns a given value to all diagonal elements.</summary>
		/// <param name="rows">Number of rows.</param>
		/// <param name="columns">Number of columns.</param>
		/// <param name="value">Value to assign to the diagnoal elements.</param>
		public Matrix(int rows, int columns, double value)
		{
			this.rows = rows;
			this.columns = columns;
			this.data = new double[rows][];

			for (int i = 0; i < rows; i++)
			{
				data[i] = new double[columns];
			}

			for (int i = 0; i < rows; i++)
			{
				data[i][i] = value;
			}
		}
	
		/// <summary>Constructs a matrix from the given array.</summary>
		/// <param name="value">The array the matrix gets constructed from.</param>
		//[CLSCompliant(false)]
		public Matrix(double[][] value)
		{
			this.rows = value.Length;
			this.columns = value[0].Length;
	
			for (int i = 0; i < rows; i++)
			{
				if (value[i].Length != columns)
				{
					throw new ArgumentException("Argument out of range."); 
				}
			}
	
			this.data = value;
		}
        #endregion // Constructors

        #region IEquatable<Matrix>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(this, (Matrix)obj);
        }

        /// <summary>
        /// Determines weather two instances are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Matrix other)
        {
            return Equals(this, other);
        }
        
        /// <summary>
        /// Determines weather two instances are equal.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
		public static bool Equals(Matrix left, Matrix right)
		{
			if (((object) left) == ((object) right))
			{
				return true;
			}

			if ((((object) left) == null) || (((object) right) == null))
			{
				return false;
			}

			if ((left.Rows != right.Rows) || (left.Columns != right.Columns))
			{
				return false;
			}

			for (int i = 0; i < left.Rows; i++)
			{
				for (int j = 0; j < left.Columns; j++)
				{
					if (left[i, j] != right[i, j])
					{
						return false;	
					}	
				}	
			}
			
			return true;
		}
        #endregion // IEquatable<Matrix>

        /// <summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
		public override int GetHashCode()
		{
			return (this.Rows + this.Columns);
		}

        #region Accessors
        public double[][] Array
		{
			get 
			{ 
				return this.data; 
			}
		}
	
		/// <summary>Returns the number of columns.</summary>
		public int Rows
		{
			get 
			{ 
				return this.rows; 
			}
		}

		/// <summary>Returns the number of columns.</summary>
		public int Columns
		{
			get 
			{ 
				return this.columns; 
			}
		}

		/// <summary>Return <see langword="true"/> if the matrix is a square matrix.</summary>
		public bool Square
		{
			get 
			{ 
				return (rows == columns); 
			}
		}

		/// <summary>Returns <see langword="true"/> if the matrix is symmetric.</summary>
		public bool Symmetric
		{
			get
			{
				if (this.Square)
				{
					for (int i = 0; i < rows; i++)
					{
						for (int j = 0; j <= i; j++)
						{
							if (data[i][j] != data[j][i])
							{
								return false;
							}
						}
					}

					return true;
				}

				return false;
			}
		}

		/// <summary>Access the value at the given location.</summary>
		public double this[int row, int column]
		{
			set 
			{ 
				this.data[row][column] = value; 
			}
			
			get 
			{ 
				return this.data[row][column]; 
			}
		}
        #endregion // Accessors

        #region Submatrix
        /// <summary>Returns a sub matrix extracted from the current matrix.</summary>
		/// <param name="startRow">Start row index</param>
		/// <param name="endRow">End row index</param>
		/// <param name="startColumn">Start column index</param>
		/// <param name="endColumn">End column index</param>
		public Matrix Submatrix(int startRow, int endRow, int startColumn, int endColumn)
		{
			if ((startRow > endRow) || (startColumn > endColumn) ||  (startRow < 0) || (startRow >= this.rows) ||  (endRow < 0) || (endRow >= this.rows) ||  (startColumn < 0) || (startColumn >= this.columns) ||  (endColumn < 0) || (endColumn >= this.columns)) 
            { 
				throw new ArgumentException("Argument out of range."); 
			} 
			
			Matrix X = new Matrix(endRow - startRow + 1, endColumn - startColumn + 1);
			double[][] x = X.Array;
			for (int i = startRow; i <= endRow; i++)
			{
				for (int j = startColumn; j <= endColumn; j++)
				{
					x[i - startRow][j - startColumn] = data[i][j];
				}
			}
					
			return X;
		}

		/// <summary>Returns a sub matrix extracted from the current matrix.</summary>
		/// <param name="rowIndexes">Array of row indices</param>
		/// <param name="columnIndexes">Array of column indices</param>
		public Matrix Submatrix(int[] rowIndexes, int[] columnIndexes)
		{
			Matrix X = new Matrix(rowIndexes.Length, columnIndexes.Length);
			double[][] x = X.Array;
			for (int i = 0; i < rowIndexes.Length; i++)
			{
				for (int j = 0; j < columnIndexes.Length; j++)
				{
                    if ((rowIndexes[i] < 0) || (rowIndexes[i] >= rows) || (columnIndexes[j] < 0) || (columnIndexes[j] >= columns))
                    { 
						throw new ArgumentException("Argument out of range."); 
                    }

					x[i][j] = data[rowIndexes[i]][columnIndexes[j]];
				}
			}

			return X;
		}

		/// <summary>Returns a sub matrix extracted from the current matrix.</summary>
		/// <param name="i0">Starttial row index</param>
		/// <param name="i1">End row index</param>
		/// <param name="c">Array of row indices</param>
		public Matrix Submatrix(int i0, int i1, int[] c)
		{
			if ((i0 > i1) || (i0 < 0) || (i0 >= this.rows) || (i1 < 0) || (i1 >= this.rows)) 
			{ 
            	throw new ArgumentException("Argument out of range."); 
			} 
			
			Matrix X = new Matrix(i1 - i0 + 1, c.Length);
			double[][] x = X.Array;
			for (int i = i0; i <= i1; i++)
			{
				for (int j = 0; j < c.Length; j++)
				{
                    if ((c[j] < 0) || (c[j] >= columns)) 
                    { 
						throw new ArgumentException("Argument out of range."); 
                    } 

					x[i - i0][j] = data[i][c[j]];
				}
			}

			return X;
		}

		/// <summary>Returns a sub matrix extracted from the current matrix.</summary>
		/// <param name="r">Array of row indices</param>
		/// <param name="j0">Start column index</param>
		/// <param name="j1">End column index</param>
		public Matrix Submatrix(int[] r, int j0, int j1)
		{
			if ((j0 > j1) || (j0 < 0) || (j0 >= columns) || (j1 < 0) || (j1 >= columns)) 
			{ 
				throw new ArgumentException("Argument out of range."); 
			} 
			
			Matrix X = new Matrix(r.Length, j1-j0+1);
			double[][] x = X.Array;
			for (int i = 0; i < r.Length; i++)
			{
				for (int j = j0; j <= j1; j++) 
				{
					if ((r[i] < 0) || (r[i] >= this.rows))
					{
						throw new ArgumentException("Argument out of range."); 
					}

					x[i][j - j0] = data[r[i]][j];
				}
			}

			return X;
		}
        #endregion // Submatrix


        /// <summary>Creates a copy of the matrix.</summary>
		public Matrix Clone()
		{
			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = data[i][j];
				}
			}

			return X;
		}

		/// <summary>Returns the transposed matrix.</summary>
		public Matrix Transpose()
		{
			Matrix X = new Matrix(columns, rows);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[j][i] = data[i][j];
				}
			}

			return X;
		}

        /// <summary>Returns the diagonal matrix.</summary>
        public Matrix DiagMat()
        {
            int size = Math.Min(rows, columns);
            Matrix X = new Matrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < size; i++)
            {
                x[i][i] = data[i][i];
            }

            return X;
        }
        
        /// <summary>Returns the matrix of absolute valued elements.</summary>
        public Matrix Abs()
        {
            Matrix X = new Matrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = Math.Abs(data[i][j]);
                }
            }

            return X;
        }
        
		/// <summary>Returns the One Norm for the matrix.</summary>
		/// <value>The maximum column sum.</value>
		public double Norm1
		{
			get
			{
				double f = 0;
				for (int j = 0; j < columns; j++)
				{
					double s = 0;
					for (int i = 0; i < rows; i++)
					{
						s += Math.Abs(data[i][j]);
					}

					f = Math.Max(f, s);
				}
				return f;
			}		
		}

		/// <summary>Returns the Infinity Norm for the matrix.</summary>
		/// <value>The maximum row sum.</value>
		public double InfinityNorm
		{
			get
			{
				double f = 0;
				for (int i = 0; i < rows; i++)
				{
					double s = 0;
					for (int j = 0; j < columns; j++)
						s += Math.Abs(data[i][j]);
					f = Math.Max(f, s);
				}
				return f;
			}
		}

		/// <summary>Returns the Frobenius Norm for the matrix.</summary>
		/// <value>The square root of sum of squares of all elements.</value>
		public double FrobeniusNorm
		{
			get
			{
				double f = 0;
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						f = Hypotenuse(f, data[i][j]);
					}
				}

				return f;
			}					
		}

		/// <summary>Unary minus.</summary>
		public static Matrix Negate(Matrix value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			int rows = value.Rows;
			int columns = value.Columns;
			double[][] data = value.Array;

			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = -data[i][j];
				}
			}

			return X;
		}

		/// <summary>Unary minus.</summary>
		public static Matrix operator-(Matrix value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			return Negate(value);
		}

		/// <summary>Matrix equality.</summary>
		public static bool operator==(Matrix left, Matrix right)
		{
			return Equals(left, right);
		}

		/// <summary>Matrix inequality.</summary>
		public static bool operator!=(Matrix left, Matrix right)
		{
			return !Equals(left, right);
		}

		/// <summary>Matrix addition.</summary>
		public static Matrix Add(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			int rows = left.Rows;
			int columns = left.Columns;
			double[][] data = left.Array;

			if ((rows != right.Rows) || (columns != right.Columns))
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}

			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = data[i][j] + right[i,j];
				}
			}
			return X;
		}

		/// <summary>Matrix addition.</summary>
		public static Matrix operator+(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			return Add(left, right);
		}

		/// <summary>Matrix subtraction.</summary>
		public static Matrix Subtract(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			int rows = left.Rows;
			int columns = left.Columns;
			double[][] data = left.Array;

			if ((rows != right.Rows) || (columns != right.Columns))
			{
				throw new ArgumentException("Matrix dimension do not match.");
			}

			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = data[i][j] - right[i,j];
				}
			}
			return X;
		}

		/// <summary>Matrix subtraction.</summary>
		public static Matrix operator-(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			return Subtract(left, right);
		}

		/// <summary>Matrix-scalar multiplication.</summary>
		public static Matrix Multiply(Matrix left, double right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			int rows = left.Rows;
			int columns = left.Columns;
			double[][] data = left.Array;

			Matrix X = new Matrix(rows, columns);

			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = data[i][j] * right;
				}
			}

			return X;
		}

		/// <summary>Matrix-scalar multiplication.</summary>
		public static Matrix operator*(Matrix left, double right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			return Multiply(left, right);
		}

		/// <summary>Matrix-matrix multiplication.</summary>
		public static Matrix Multiply(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			int rows = left.Rows;
			double[][] data = left.Array;

			if (right.Rows != left.columns)
			{
				throw new ArgumentException("Matrix dimensions are not valid.");
			}

			int columns = right.Columns;
			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;

			int size = left.columns;
			double[] column = new double[size];
			for (int j = 0; j < columns; j++)
			{
				for (int k = 0; k < size; k++)
				{
					column[k] = right[k,j];
				}
				for (int i = 0; i < rows; i++)
				{
					double[] row = data[i];
					double s = 0;
					for (int k = 0; k < size; k++)
					{
						s += row[k] * column[k];
					}
					x[i][j] = s;
				} 
			}

			return X;
		}

		/// <summary>Matrix-matrix multiplication.</summary>
		public static Matrix operator*(Matrix left, Matrix right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}
			
			return Multiply(left, right);
		}
        
		/// <summary>Returns the LHS solution vetor if the matrix is square or the least squares solution otherwise.</summary>
		public Matrix Solve(Matrix rightHandSide)
		{
			return (rows == columns) ? new LuDecomposition(this).Solve(rightHandSide) : new QrDecomposition(this).Solve(rightHandSide);
		}

		/// <summary>Inverse of the matrix if matrix is square, pseudoinverse otherwise.</summary>
		public Matrix Inverse
		{
			get 
			{ 
				return this.Solve(Diagonal(rows, rows, 1.0)); 
			}
		}

		/// <summary>Determinant if matrix is square.</summary>
		public double Determinant
		{
			get 
			{ 
				return new LuDecomposition(this).Determinant; 
			}
		}

		/// <summary>Returns the trace of the matrix.</summary>
		/// <returns>Sum of the diagonal elements.</returns>
		public double Trace
		{
			get
			{
				double trace = 0;
				for (int i = 0; i < Math.Min(rows, columns); i++)
				{
					trace += data[i][i];
				}
				return trace;
			}
		}
        
		/// <summary>Returns a matrix filled with random values.</summary>
		public static Matrix Random(int rows, int columns)
		{
			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = random.NextDouble();
				}
			}
			return X;
		}

		/// <summary>Returns a diagonal matrix of the given size.</summary>
		public static Matrix Diagonal(int rows, int columns, double value)
		{
			Matrix X = new Matrix(rows, columns);
			double[][] x = X.Array;
			for (int i = 0; i < rows; i++)
			{
				for (int j = 0; j < columns; j++)
				{
					x[i][j] = ((i == j) ? value : 0.0);
				}
			}
			return X;
		}

        /// <summary>Returns a entries of zero matrix of the given size.</summary>
        public static Matrix Zeros(int rows, int columns)
        {
            Matrix X = new Matrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = 0.0;
                }
            }
            return X;
        }
        
        /// <summary>Returns a entries of one matrix of the given size.</summary>
        public static Matrix Ones(int rows, int columns)
        {
            Matrix X = new Matrix(rows, columns);
            double[][] x = X.Array;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    x[i][j] = 1.0;
                }
            }
            return X;
        }
        
		/// <summary>Returns the matrix in a textual form.</summary>
		public override string ToString()
		{
			using (StringWriter writer = new StringWriter(CultureInfo.InvariantCulture))
			{
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						writer.Write(this.data[i][j] + " ");
					}
	
					writer.WriteLine();
				}

				return writer.ToString();
			}
		}

		private static double Hypotenuse(double a, double b) 
		{
			if (Math.Abs(a) > Math.Abs(b))
			{
				double r = b / a;
				return Math.Abs(a) * Math.Sqrt(1 + r * r);
			}

			if (b != 0)
			{
				double r = a / b;
				return Math.Abs(b) * Math.Sqrt(1 + r * r);
			}

			return 0.0;
		}

    }

    /// <summary>
    ///   LU decomposition of a rectangular matrix.
    /// </summary>
    /// <remarks>
    ///   For an m-by-n matrix <c>A</c> with m >= n, the LU decomposition is an m-by-n
    ///   unit lower triangular matrix <c>L</c>, an n-by-n upper triangular matrix <c>U</c>,
    ///   and a permutation vector <c>piv</c> of length m so that <c>A(piv)=L*U</c>.
    ///   If m &lt; n, then <c>L</c> is m-by-m and <c>U</c> is m-by-n.
    ///   The LU decompostion with pivoting always exists, even if the matrix is
    ///   singular, so the constructor will never fail.  The primary use of the
    ///   LU decomposition is in the solution of square systems of simultaneous
    ///   linear equations. This will fail if <see cref="NonSingular"/> returns <see langword="false"/>.
    /// </remarks>
    public class LuDecomposition
    {
        private Matrix LU;
        private int pivotSign;
        private int[] pivotVector;

        /// <summary>Construct a LU decomposition.</summary>	
        public LuDecomposition(Matrix value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.LU = (Matrix)value.Clone();
            double[][] lu = LU.Array;
            int rows = value.Rows;
            int columns = value.Columns;
            pivotVector = new int[rows];
            for (int i = 0; i < rows; i++)
            {
                pivotVector[i] = i;
            }

            pivotSign = 1;
            double[] LUrowi;
            double[] LUcolj = new double[rows];

            // Outer loop.
            for (int j = 0; j < columns; j++)
            {
                // Make a copy of the j-th column to localize references.
                for (int i = 0; i < rows; i++)
                {
                    LUcolj[i] = lu[i][j];
                }

                // Apply previous transformations.
                for (int i = 0; i < rows; i++)
                {
                    LUrowi = lu[i];

                    // Most of the time is spent in the following dot product.
                    int kmax = Math.Min(i, j);
                    double s = 0.0;
                    for (int k = 0; k < kmax; k++)
                    {
                        s += LUrowi[k] * LUcolj[k];
                    }
                    LUrowi[j] = LUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                int p = j;
                for (int i = j + 1; i < rows; i++)
                {
                    if (Math.Abs(LUcolj[i]) > Math.Abs(LUcolj[p]))
                    {
                        p = i;
                    }
                }

                if (p != j)
                {
                    for (int k = 0; k < columns; k++)
                    {
                        double t = lu[p][k];
                        lu[p][k] = lu[j][k];
                        lu[j][k] = t;
                    }

                    int v = pivotVector[p];
                    pivotVector[p] = pivotVector[j];
                    pivotVector[j] = v;

                    pivotSign = -pivotSign;
                }

                // Compute multipliers.

                if (j < rows & lu[j][j] != 0.0)
                {
                    for (int i = j + 1; i < rows; i++)
                    {
                        lu[i][j] /= lu[j][j];
                    }
                }
            }
        }

        /// <summary>Returns if the matrix is non-singular.</summary>
        public bool NonSingular
        {
            get
            {
                for (int j = 0; j < LU.Columns; j++)
                    if (LU[j, j] == 0)
                        return false;
                return true;
            }
        }

        /// <summary>Returns the determinant of the matrix.</summary>
        public double Determinant
        {
            get
            {
                if (LU.Rows != LU.Columns) throw new ArgumentException("Matrix must be square.");
                double determinant = (double)pivotSign;
                for (int j = 0; j < LU.Columns; j++)
                    determinant *= LU[j, j];
                return determinant;
            }
        }

        /// <summary>Returns the lower triangular factor <c>L</c> with <c>A=LU</c>.</summary>
        public Matrix LowerTriangularFactor
        {
            get
            {
                int rows = LU.Rows;
                int columns = LU.Columns;
                Matrix X = new Matrix(rows, columns);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        if (i > j)
                            X[i, j] = LU[i, j];
                        else if (i == j)
                            X[i, j] = 1.0;
                        else
                            X[i, j] = 0.0;
                return X;
            }
        }

        /// <summary>Returns the lower triangular factor <c>L</c> with <c>A=LU</c>.</summary>
        public Matrix UpperTriangularFactor
        {
            get
            {
                int rows = LU.Rows;
                int columns = LU.Columns;
                Matrix X = new Matrix(rows, columns);
                for (int i = 0; i < rows; i++)
                    for (int j = 0; j < columns; j++)
                        if (i <= j)
                            X[i, j] = LU[i, j];
                        else
                            X[i, j] = 0.0;
                return X;
            }
        }

        /// <summary>Returns the pivot permuation vector.</summary>
        public double[] PivotPermutationVector
        {
            get
            {
                int rows = LU.Rows;

                double[] p = new double[rows];
                for (int i = 0; i < rows; i++)
                {
                    p[i] = (double)this.pivotVector[i];
                }

                return p;
            }
        }

        /// <summary>Solves a set of equation systems of type <c>A * X = B</c>.</summary>
        /// <param name="value">Right hand side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>Matrix <c>X</c> so that <c>L * U * X = B</c>.</returns>
        public Matrix Solve(Matrix value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.Rows != this.LU.Rows)
            {
                throw new ArgumentException("Invalid matrix dimensions.", "value");
            }

            if (!this.NonSingular)
            {
                return null;
                throw new InvalidOperationException("Matrix is singular");
            }

            // Copy right hand side with pivoting
            int count = value.Columns;
            Matrix X = value.Submatrix(pivotVector, 0, count - 1);

            int rows = LU.Rows;
            int columns = LU.Columns;
            double[][] lu = LU.Array;

            // Solve L*Y = B(piv,:)
            for (int k = 0; k < columns; k++)
            {
                for (int i = k + 1; i < columns; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        X[i, j] -= X[k, j] * lu[i][k];
                    }
                }
            }

            // Solve U*X = Y;
            for (int k = columns - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                {
                    X[k, j] /= lu[k][k];
                }

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        X[i, j] -= X[k, j] * lu[i][k];
                    }
                }
            }

            return X;
        }
    }

    /// <summary>
    ///	  QR decomposition for a rectangular matrix.
    /// </summary>
    /// <remarks>
    ///   For an m-by-n matrix <c>A</c> with <c>m &gt;= n</c>, the QR decomposition is an m-by-n
    ///   orthogonal matrix <c>Q</c> and an n-by-n upper triangular 
    ///   matrix <c>R</c> so that <c>A = Q * R</c>.
    ///   The QR decompostion always exists, even if the matrix does not have
    ///   full rank, so the constructor will never fail.  The primary use of the
    ///   QR decomposition is in the least squares solution of nonsquare systems
    ///   of simultaneous linear equations.
    ///   This will fail if <see cref="FullRank"/> returns <see langword="false"/>.
    /// </remarks>
    public class QrDecomposition
    {
        private Matrix QR;
        private double[] Rdiag;

        /// <summary>Construct a QR decomposition.</summary>	
        public QrDecomposition(Matrix value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            this.QR = (Matrix)value.Clone();
            double[][] qr = this.QR.Array;
            int m = value.Rows;
            int n = value.Columns;
            this.Rdiag = new double[n];

            for (int k = 0; k < n; k++)
            {
                // Compute 2-norm of k-th column without under/overflow.
                double nrm = 0;
                for (int i = k; i < m; i++)
                {
                    nrm = Hypotenuse(nrm, qr[i][k]);
                }

                if (nrm != 0.0)
                {
                    // Form k-th Householder vector.
                    if (qr[k][k] < 0)
                    {
                        nrm = -nrm;
                    }

                    for (int i = k; i < m; i++)
                    {
                        qr[i][k] /= nrm;
                    }

                    qr[k][k] += 1.0;

                    // Apply transformation to remaining columns.
                    for (int j = k + 1; j < n; j++)
                    {
                        double s = 0.0;

                        for (int i = k; i < m; i++)
                        {
                            s += qr[i][k] * qr[i][j];
                        }

                        s = -s / qr[k][k];

                        for (int i = k; i < m; i++)
                        {
                            qr[i][j] += s * qr[i][k];
                        }
                    }
                }

                this.Rdiag[k] = -nrm;
            }
        }

        /// <summary>Least squares solution of <c>A * X = B</c></summary>
        /// <param name="value">Right-hand-side matrix with as many rows as <c>A</c> and any number of columns.</param>
        /// <returns>A matrix that minimized the two norm of <c>Q * R * X - B</c>.</returns>
        /// <exception cref="T:System.ArgumentException">Matrix row dimensions must be the same.</exception>
        /// <exception cref="T:System.InvalidOperationException">Matrix is rank deficient.</exception>
        public Matrix Solve(Matrix value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            if (value.Rows != QR.Rows)
            {
                throw new ArgumentException("Matrix row dimensions must agree.");
            }

            if (!this.FullRank)
            {
                throw new InvalidOperationException("Matrix is rank deficient.");
            }

            // Copy right hand side
            int count = value.Columns;
            Matrix X = value.Clone();
            int m = QR.Rows;
            int n = QR.Columns;
            double[][] qr = QR.Array;

            // Compute Y = transpose(Q)*B
            for (int k = 0; k < n; k++)
            {
                for (int j = 0; j < count; j++)
                {
                    double s = 0.0;

                    for (int i = k; i < m; i++)
                    {
                        s += qr[i][k] * X[i, j];
                    }

                    s = -s / qr[k][k];

                    for (int i = k; i < m; i++)
                    {
                        X[i, j] += s * qr[i][k];
                    }
                }
            }

            // Solve R*X = Y;
            for (int k = n - 1; k >= 0; k--)
            {
                for (int j = 0; j < count; j++)
                {
                    X[k, j] /= Rdiag[k];
                }

                for (int i = 0; i < k; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        X[i, j] -= X[k, j] * qr[i][k];
                    }
                }
            }

            return X.Submatrix(0, n - 1, 0, count - 1);
        }

        /// <summary>Shows if the matrix <c>A</c> is of full rank.</summary>
        /// <value>The value is <see langword="true"/> if <c>R</c>, and hence <c>A</c>, has full rank.</value>
        public bool FullRank
        {
            get
            {
                int columns = this.QR.Columns;
                for (int i = 0; i < columns; i++)
                {
                    if (this.Rdiag[i] == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>Returns the upper triangular factor <c>R</c>.</summary>
        public Matrix UpperTriangularFactor
        {
            get
            {
                int n = this.QR.Columns;
                Matrix X = new Matrix(n, n);
                double[][] x = X.Array;
                double[][] qr = QR.Array;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (i < j)
                        {
                            x[i][j] = qr[i][j];
                        }
                        else if (i == j)
                        {
                            x[i][j] = Rdiag[i];
                        }
                        else
                        {
                            x[i][j] = 0.0;
                        }
                    }
                }

                return X;
            }
        }

        /// <summary>Returns the orthogonal factor <c>Q</c>.</summary>
        public Matrix OrthogonalFactor
        {
            get
            {
                Matrix X = new Matrix(QR.Rows, QR.Columns);
                double[][] x = X.Array;
                double[][] qr = QR.Array;
                for (int k = QR.Columns - 1; k >= 0; k--)
                {
                    for (int i = 0; i < QR.Rows; i++)
                    {
                        x[i][k] = 0.0;
                    }

                    x[k][k] = 1.0;
                    for (int j = k; j < QR.Columns; j++)
                    {
                        if (qr[k][k] != 0)
                        {
                            double s = 0.0;

                            for (int i = k; i < QR.Rows; i++)
                            {
                                s += qr[i][k] * x[i][j];
                            }

                            s = -s / qr[k][k];

                            for (int i = k; i < QR.Rows; i++)
                            {
                                x[i][j] += s * qr[i][k];
                            }
                        }
                    }
                }

                return X;
            }
        }

        private static double Hypotenuse(double a, double b)
        {
            if (Math.Abs(a) > Math.Abs(b))
            {
                double r = b / a;
                return Math.Abs(a) * Math.Sqrt(1 + r * r);
            }

            if (b != 0)
            {
                double r = a / b;
                return Math.Abs(b) * Math.Sqrt(1 + r * r);
            }

            return 0.0;
        }
    }


}
