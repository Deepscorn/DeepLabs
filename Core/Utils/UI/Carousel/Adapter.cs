// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

namespace Assets.Sources.Util.UI.Carousel
{
    // inherits MonoBehavior to be able to be edited from Editor and accessed by CarouselController by GetComponent
    public abstract class Adapter: MonoBehaviour
    {
        /**
         * Called when data is no longer valid. Adapter then resets it's state with current
         * data. Must be called before using adapter
         */
        public abstract void Reset();

        /**
     * How many items are in the data set represented by this Adapter.
     * 
     * @return Count of items.
     */
        public abstract int GetCount();
        
        /**
     * Get a View that displays the data at the specified position in the data set. You can either
     * create a View manually or inflate it from an XML layout file. When the View is inflated, the
     * parent View (GridView, ListView...) will apply default layout parameters unless you use
     * {@link android.view.LayoutInflater#inflate(int, android.view.ViewGroup, boolean)}
     * to specify a root view and to prevent attachment to the root.
     * 
     * @param position The position of the item within the adapter's data set of the item whose view
     *        we want.
     * @param convertView The old view to reuse, if possible. Note: You should check that this view
     *        is non-null and of an appropriate type before using. If it is not possible to convert
     *        this view to display the correct data, this method can create a new view.
     *        Heterogeneous lists can specify their number of view types, so that this View is
     *        always of the right type (see {@link #getViewTypeCount()} and
     *        {@link #getItemViewType(int)}).
     * @param parent The parent that this view will eventually be attached to
     * @return A View corresponding to the data at the specified position.
     */
        public abstract GameObject GetView(int position, GameObject convertView, GameObject parent);
        
        /**
     * Get the type of View that will be created by {@link #getView} for the specified item.
     * 
     * @param position The position of the item within the adapter's data set whose view type we
     *        want.
     * @return An integer representing the type of View. Two views should share the same type if one
     *         can be converted to the other in {@link #getView}. Note: Integers must be in the
     *         range 0 to {@link #getViewTypeCount} - 1. {@link #IGNORE_ITEM_VIEW_TYPE} can
     *         also be returned.
     * @see #IGNORE_ITEM_VIEW_TYPE
     */
        public abstract int GetItemViewType(int position);
        
        /**
     * <p>
     * Returns the number of types of Views that will be created by
     * {@link #getView}. Each type represents a set of views that can be
     * converted in {@link #getView}. If the adapter always returns the same
     * type of View for all items, this method should return 1.
     * </p>
     * <p>
     * This method will only be called when when the adapter is set on the
     * the {@link AdapterView}.
     * </p>
     * 
     * @return The number of types of Views that will be created by this adapter
     */
        public abstract int GetViewTypeCount();
    }
}

