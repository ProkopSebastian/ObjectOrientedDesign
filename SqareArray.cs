using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace projob_Projekt
{

    public class SquareArray<T>
    {
        private T[,] data;
        private int size;
        private int count;
        private int row;
        private int col;

        public SquareArray(int initialSize = 1)
        {
            data = new T[initialSize, initialSize];
            size = initialSize;
            count = 0;
        }

        public void Insert(T value)
        {
            data[row, col] = value;

            // on diagonal
            if (row == col)
            {
                //ResizeArray();
                T[,] newData = new T[size + 1, size + 1];

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        newData[i, j] = data[i, j];
                    }
                }

                data = newData;
                size++;
                // resize
                row = 0;
                col++;
            }

            else if (row < col)
            {
                if (row == col - 1)
                {
                    row = col;
                    col = 0;
                }
                else
                    row++;
            }
            else
                col++; // tutaj zmieniałem coś

            count++;
        }

        public void Usun()
        {
            if (count <= 0)
                throw new Exception();

            int lastItemX;
            int lasItemY;

            // on diagonal
            if (row == col)
            {
                lastItemX = row - 1;
                lasItemY = col;

                row--;
            }
            // wpisujemy do kolumny
            else if (row < col)
            {
                // zaczelismy nowa kolumne
                if (row == 0)
                    row = lastItemX = lasItemY = --col;
                else
                {
                    lasItemY = col;
                    lastItemX = --row;
                }
            }
            // zaczelismy nowy wiersz
            else
            {
                if (col == 0)
                {
                    lasItemY = row;
                    lastItemX = --row;
                }
                else
                {
                    lasItemY = --col;
                    lastItemX = row;
                }
            }
            count--;
        }

        public IIterator<T> GetForwardIterator()
        {
            return new SquareArrayForwardIterator<T>(this);
        }

        public IIterator<T> GetReverseIterator()
        {
            return new SquareArrayReverseIterator<T>(this);
        }

        public bool IsEmpty()
        {
            return count == 0;
        }

        public int GetSize()
        {
            return count; // tutaj był błąd przez moje nienajlepsze nazwenictwo
        }

        public T GetValue(int x, int y)
        {
            return data[x,y];
        }

        public (int, int) GetLastIndex()
        {
            int lastX;
            int lastY;

            if (row == col)
            {
                lastX = row;
                lastY = col - 1;
            }
            // wpisujemy do kolumny
            else if (row < col)
            {
                // zaczelismy nowa kolumne
                if (row == 0)
                    lastX = lastY = col - 1;
                else
                {
                    lastY = col;
                    lastX = row - 1;
                }
            }
            // zaczelismy nowy wiersz
            else
            {
                if (col == 0)
                {
                    lastY = row;
                    lastX = row - 1;
                }
                else
                {
                    lastY = col - 1;
                    lastX = row;
                }
            }
            return (lastX, lastY);
        }

        // żeby działał FOREACH
        public IEnumerator<T> GetEnumerator()
        {
            return new SquareArrayForwardIterator<T>(this).GetValues().GetEnumerator();
        }

        // Żeby działał countIf
        public int CountIf(Func<T, bool> condition)
        {
            int count = 0;
            foreach (var value in this.GetForwardIterator().GetValues())
            {
                if (condition(value))
                {
                    count++;
                }
            }
            return count;
        }
        // jeszcze Find do zrobienia


    }

    // Iteratory
    public interface IIterator<T>
    {
        public IEnumerable<T> GetValues();
    }

    public class SquareArrayForwardIterator<T> : IIterator<T>
    {
        int index;
        int indexX;
        int indexY;
        SquareArray<T> Idata;

        public SquareArrayForwardIterator(SquareArray<T> array)
        {
            Idata = array;
            index = indexX = indexY = 0;
        }

        public IEnumerable<T> GetValues()
        {
            while (index < Idata.GetSize())
            {
                yield return Idata.GetValue(indexX, indexY);
                if (indexX == indexY)
                {
                    indexX = 0;
                    indexY++;
                }

                else if (indexX < indexY)
                {
                    if (indexX == indexY - 1)
                    {
                        indexX = indexY;
                        indexY = 0;
                    }
                    else
                        indexX++;
                }
                else
                {
                    indexY++;
                }
                index++;
            }
        }
    }

    public class SquareArrayReverseIterator<T> : IIterator<T>
    {
        int index;
        int indexX;
        int indexY;
        SquareArray<T> Idata;

        public SquareArrayReverseIterator(SquareArray<T> array)
        {
            Idata = array;
            index = array.GetSize() - 1;
            (indexX, indexY) = Idata.GetLastIndex();
        }

        public IEnumerable<T> GetValues()
        {
            while (index >= 0)
            {
                yield return Idata.GetValue(indexX, indexY);

                if (indexX == indexY)
                {
                    // one col back
                    indexY--;
                }
                else if (indexX < indexY)
                {
                    // last item in col
                    if (indexX == 0)
                    {
                        indexY = indexX = indexY - 1;
                    }
                    else
                        indexX--;
                }
                else
                {
                    if (indexY == 0)
                    {
                        indexY = indexX;
                        indexX--;
                    }
                    else
                        indexY--;
                }

                index--;
            }
        }

    }
}